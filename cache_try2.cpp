#include "cachelab.h"
#include <stdint.h>
#include <stdbool.h>
#include <stdio.h>

// 全局统计量
int l1d_hits = 0;
int l1d_misses = 0;
int l1d_evictions = 0;
int l1i_hits = 0;
int l1i_misses = 0;
int l1i_evictions = 0;
int l2_hits = 0;
int l2_misses = 0;
int l2_evictions = 0;
int l3_hits = 0;
int l3_misses = 0;
int l3_evictions = 0;

// 全局时间戳计数器
uint64_t current_tick = 0;

// 辅助函数声明
uint64_t getL1Tag(uint64_t addr);
uint64_t getL1Set(uint64_t addr);
uint64_t getL2Tag(uint64_t addr);
uint64_t getL2Set(uint64_t addr);
uint64_t getL3Tag(uint64_t addr);
uint64_t getL3Set(uint64_t addr);
void updateLRU(CacheLine *line);
int findLRU(CacheLine cache[], int line_num);
void backInvalidate(uint64_t addr, int level);
bool accessL1D(uint64_t addr, char op);
bool accessL1I(uint64_t addr);
bool accessL2(uint64_t addr, char op);
bool accessL3(uint64_t addr, char op);
void loadToL1D(uint64_t addr, char op);
void loadToL1I(uint64_t addr);
void loadToL2(uint64_t addr, char op);
void loadToL3(uint64_t addr, char op);
void handleLoad(uint64_t addr);
void handleStore(uint64_t addr);
void handleModify(uint64_t addr, uint32_t len);

// 缓存初始化
void cacheInit() {
    current_tick = 0;
    
    // L1D和L1I缓存初始化
    for (int i = 0; i < L1_SET_NUM; i++) {
        for (int j = 0; j < L1_LINE_NUM; j++) {
            l1dcache[i][j].valid = false;
            l1dcache[i][j].dirty = false;
            l1dcache[i][j].tag = 0;
            l1dcache[i][j].latest_used = 0;
            
            l1icache[i][j].valid = false;
            l1icache[i][j].dirty = false;
            l1icache[i][j].tag = 0;
            l1icache[i][j].latest_used = 0;
        }
    }
    
    // L2缓存初始化
    for (int i = 0; i < L2_SET_NUM; i++) {
        for (int j = 0; j < L2_LINE_NUM; j++) {
            l2ucache[i][j].valid = false;
            l2ucache[i][j].dirty = false;
            l2ucache[i][j].tag = 0;
            l2ucache[i][j].latest_used = 0;
        }
    }
    
    // L3缓存初始化
    for (int i = 0; i < L3_SET_NUM; i++) {
        for (int j = 0; j < L3_LINE_NUM; j++) {
            l3ucache[i][j].valid = false;
            l3ucache[i][j].dirty = false;
            l3ucache[i][j].tag = 0;
            l3ucache[i][j].latest_used = 0;
        }
    }
}

// 地址转换函数
uint64_t getL1Tag(uint64_t addr) { return addr >> (3 + 2); }
uint64_t getL1Set(uint64_t addr) { return (addr >> 3) & 0x3; }
uint64_t getL2Tag(uint64_t addr) { return addr >> (3 + 3); }
uint64_t getL2Set(uint64_t addr) { return (addr >> 3) & 0x7; }
uint64_t getL3Tag(uint64_t addr) { return addr >> (4 + 4); }
uint64_t getL3Set(uint64_t addr) { return (addr >> 4) & 0xF; }

// 更新LRU计数器
void updateLRU(CacheLine *line) {
    line->latest_used = ++current_tick;
}

// 查找LRU行
int findLRU(CacheLine cache[], int line_num) {
    int lru_idx = 0;
    uint64_t min_tick = cache[0].latest_used;
    
    for (int i = 1; i < line_num; i++) {
        if (!cache[i].valid) return i;
        if (cache[i].latest_used < min_tick) {
            min_tick = cache[i].latest_used;
            lru_idx = i;
        }
    }
    return lru_idx;
}

// 反向无效化
void backInvalidate(uint64_t addr, int level) {
    if (level >= 2) {
        // 无效化L1
        uint64_t l1_tag = getL1Tag(addr);
        uint64_t l1_set = getL1Set(addr);
        
        for (int i = 0; i < L1_LINE_NUM; i++) {
            if (l1dcache[l1_set][i].valid && l1dcache[l1_set][i].tag == l1_tag) {
                if (l1dcache[l1_set][i].dirty) {
                    // 写回L2并标记为dirty
                    uint64_t writeback_addr = (l1dcache[l1_set][i].tag << (3 + 2)) | (l1_set << 3);
                    bool l2_hit = accessL2(writeback_addr, 'S');
                    if (l2_hit) {
                        uint64_t l2_tag = getL2Tag(writeback_addr);
                        uint64_t l2_set = getL2Set(writeback_addr);
                        for (int j = 0; j < L2_LINE_NUM; j++) {
                            if (l2ucache[l2_set][j].valid && l2ucache[l2_set][j].tag == l2_tag) {
                                l2ucache[l2_set][j].dirty = true;
                                break;
                            }
                        }
                    } else {
                        loadToL2(writeback_addr, 'S');
                        // 找到刚加载的行并标记dirty
                        uint64_t l2_tag = getL2Tag(writeback_addr);
                        uint64_t l2_set = getL2Set(writeback_addr);
                        for (int j = 0; j < L2_LINE_NUM; j++) {
                            if (l2ucache[l2_set][j].valid && l2ucache[l2_set][j].tag == l2_tag) {
                                l2ucache[l2_set][j].dirty = true;
                                break;
                            }
                        }
                    }
                }
                l1dcache[l1_set][i].valid = false;
                l1dcache[l1_set][i].dirty = false;
            }
            
            if (l1icache[l1_set][i].valid && l1icache[l1_set][i].tag == l1_tag) {
                l1icache[l1_set][i].valid = false;
            }
        }
    }
    
    if (level >= 3) {
        // 无效化L2
        uint64_t l2_tag = getL2Tag(addr);
        uint64_t l2_set = getL2Set(addr);
        
        for (int i = 0; i < L2_LINE_NUM; i++) {
            if (l2ucache[l2_set][i].valid && l2ucache[l2_set][i].tag == l2_tag) {
                if (l2ucache[l2_set][i].dirty) {
                    // 写回L3并标记为dirty
                    uint64_t writeback_addr = (l2ucache[l2_set][i].tag << (3 + 3)) | (l2_set << 3);
                    bool l3_hit = accessL3(writeback_addr, 'S');
                    if (l3_hit) {
                        uint64_t l3_tag = getL3Tag(writeback_addr);
                        uint64_t l3_set = getL3Set(writeback_addr);
                        for (int j = 0; j < L3_LINE_NUM; j++) {
                            if (l3ucache[l3_set][j].valid && l3ucache[l3_set][j].tag == l3_tag) {
                                l3ucache[l3_set][j].dirty = true;
                                break;
                            }
                        }
                    } else {
                        loadToL3(writeback_addr, 'S');
                        // 找到刚加载的行并标记dirty
                        uint64_t l3_tag = getL3Tag(writeback_addr);
                        uint64_t l3_set = getL3Set(writeback_addr);
                        for (int j = 0; j < L3_LINE_NUM; j++) {
                            if (l3ucache[l3_set][j].valid && l3ucache[l3_set][j].tag == l3_tag) {
                                l3ucache[l3_set][j].dirty = true;
                                break;
                            }
                        }
                    }
                }
                l2ucache[l2_set][i].valid = false;
                l2ucache[l2_set][i].dirty = false;
                
                // 递归无效化L1
                backInvalidate((l2ucache[l2_set][i].tag << (3 + 3)) | (l2_set << 3), 2);
            }
        }
    }
}

// L1D缓存访问
bool accessL1D(uint64_t addr, char op) {
    uint64_t tag = getL1Tag(addr);
    uint64_t set = getL1Set(addr);
    
    for (int i = 0; i < L1_LINE_NUM; i++) {
        if (l1dcache[set][i].valid && l1dcache[set][i].tag == tag) {
            l1d_hits++;
            updateLRU(&l1dcache[set][i]);
            if (op == 'S' || op == 'M') {
                l1dcache[set][i].dirty = true;
            }
            return true;
        }
    }
    l1d_misses++;
    return false;
}

// L1I缓存访问
bool accessL1I(uint64_t addr) {
    uint64_t tag = getL1Tag(addr);
    uint64_t set = getL1Set(addr);
    
    for (int i = 0; i < L1_LINE_NUM; i++) {
        if (l1icache[set][i].valid && l1icache[set][i].tag == tag) {
            l1i_hits++;
            updateLRU(&l1icache[set][i]);
            return true;
        }
    }
    l1i_misses++;
    return false;
}

// L2缓存访问
bool accessL2(uint64_t addr, char op) {
    uint64_t tag = getL2Tag(addr);
    uint64_t set = getL2Set(addr);
    
    for (int i = 0; i < L2_LINE_NUM; i++) {
        if (l2ucache[set][i].valid && l2ucache[set][i].tag == tag) {
            l2_hits++;
            updateLRU(&l2ucache[set][i]);
            if (op == 'S' || op == 'M') {
                l2ucache[set][i].dirty = true;
            }
            return true;
        }
    }
    l2_misses++;
    return false;
}

// L3缓存访问
bool accessL3(uint64_t addr, char op) {
    uint64_t tag = getL3Tag(addr);
    uint64_t set = getL3Set(addr);
    
    for (int i = 0; i < L3_LINE_NUM; i++) {
        if (l3ucache[set][i].valid && l3ucache[set][i].tag == tag) {
            l3_hits++;
            updateLRU(&l3ucache[set][i]);
            if (op == 'S' || op == 'M') {
                l3ucache[set][i].dirty = true;
            }
            return true;
        }
    }
    l3_misses++;
    return false;
}

// 加载数据到L1D
void loadToL1D(uint64_t addr, char op) {
    uint64_t tag = getL1Tag(addr);
    uint64_t set = getL1Set(addr);
    int line = findLRU(l1dcache[set], L1_LINE_NUM);
    
    if (l1dcache[set][line].valid) {
        l1d_evictions++;
        
        if (l1dcache[set][line].dirty) {
            uint64_t evicted_addr = (l1dcache[set][line].tag << (3 + 2)) | (set << 3);
            bool l2_hit = accessL2(evicted_addr, 'S');
            if (!l2_hit) {
                loadToL2(evicted_addr, 'S');
            }
            // 确保L2中对应行被标记为dirty
            uint64_t l2_tag = getL2Tag(evicted_addr);
            uint64_t l2_set = getL2Set(evicted_addr);
            for (int i = 0; i < L2_LINE_NUM; i++) {
                if (l2ucache[l2_set][i].valid && l2ucache[l2_set][i].tag == l2_tag) {
                    l2ucache[l2_set][i].dirty = true;
                    break;
                }
            }
        }
    }
    
    l1dcache[set][line].valid = true;
    l1dcache[set][line].tag = tag;
    l1dcache[set][line].dirty = (op == 'S' || op == 'M');
    updateLRU(&l1dcache[set][line]);
}

// 加载指令到L1I
void loadToL1I(uint64_t addr) {
    uint64_t tag = getL1Tag(addr);
    uint64_t set = getL1Set(addr);
    int line = findLRU(l1icache[set], L1_LINE_NUM);
    
    if (l1icache[set][line].valid) {
        l1i_evictions++;
    }
    
    l1icache[set][line].valid = true;
    l1icache[set][line].tag = tag;
    updateLRU(&l1icache[set][line]);
}

// 加载数据到L2
void loadToL2(uint64_t addr, char op) {
    uint64_t tag = getL2Tag(addr);
    uint64_t set = getL2Set(addr);
    int line = findLRU(l2ucache[set], L2_LINE_NUM);
    
    if (l2ucache[set][line].valid) {
        l2_evictions++;
        
        if (l2ucache[set][line].dirty) {
            uint64_t evicted_addr = (l2ucache[set][line].tag << (3 + 3)) | (set << 3);
            bool l3_hit = accessL3(evicted_addr, 'S');
            if (!l3_hit) {
                loadToL3(evicted_addr, 'S');
            }
            // 确保L3中对应行被标记为dirty
            uint64_t l3_tag = getL3Tag(evicted_addr);
            uint64_t l3_set = getL3Set(evicted_addr);
            for (int i = 0; i < L3_LINE_NUM; i++) {
                if (l3ucache[l3_set][i].valid && l3ucache[l3_set][i].tag == l3_tag) {
                    l3ucache[l3_set][i].dirty = true;
                    break;
                }
            }
        }
        
        backInvalidate((l2ucache[set][line].tag << (3 + 3)) | (set << 3), 2);
    }
    
    l2ucache[set][line].valid = true;
    l2ucache[set][line].tag = tag;
    l2ucache[set][line].dirty = (op == 'S' || op == 'M');
    updateLRU(&l2ucache[set][line]);
}

// 加载数据到L3
void loadToL3(uint64_t addr, char op) {
    uint64_t tag = getL3Tag(addr);
    uint64_t set = getL3Set(addr);
    int line = findLRU(l3ucache[set], L3_LINE_NUM);
    
    if (l3ucache[set][line].valid) {
        l3_evictions++;
        
        if (l3ucache[set][line].dirty) {
            // 在实际系统中，这里应该写回内存
            // 在模拟器中，我们只统计eviction，不实际模拟内存
        }
        
        backInvalidate((l3ucache[set][line].tag << (4 + 4)) | (set << 4), 3);
    }
    
    l3ucache[set][line].valid = true;
    l3ucache[set][line].tag = tag;
    l3ucache[set][line].dirty = (op == 'S' || op == 'M');
    updateLRU(&l3ucache[set][line]);
}

// 处理Load操作
void handleLoad(uint64_t addr) {
    if (!accessL1D(addr, 'L')) {
        if (!accessL2(addr, 'L')) {
            if (!accessL3(addr, 'L')) {
                loadToL3(addr, 'L');
            }
            loadToL2(addr, 'L');
        }
        loadToL1D(addr, 'L');
    }
}

// 处理Store操作
void handleStore(uint64_t addr) {
    if (!accessL1D(addr, 'S')) {
        if (!accessL2(addr, 'S')) {
            if (!accessL3(addr, 'S')) {
                loadToL3(addr, 'S');
            }
            loadToL2(addr, 'S');
        }
        loadToL1D(addr, 'S');
    }
}

// 处理Modify操作
void handleModify(uint64_t addr, uint32_t len) {
    // Load部分
    bool l1_hit = accessL1D(addr, 'L');
    if (!l1_hit) {
        bool l2_hit = accessL2(addr, 'L');
        if (!l2_hit) {
            bool l3_hit = accessL3(addr, 'L');
            if (!l3_hit) {
                loadToL3(addr, 'L');
            }
            loadToL2(addr, 'L');
        }
        loadToL1D(addr, 'L');
    }
    
    // Store部分（不更新统计量）
    accessL1D(addr, 'S');
}

// 缓存访问主函数
void cacheAccess(char op, uint64_t addr, uint32_t len) {
    switch (op) {
        case 'I': // 指令加载
            if (!accessL1I(addr)) {
                if (!accessL2(addr, 'L')) {
                    if (!accessL3(addr, 'L')) {
                        loadToL3(addr, 'L');
                    }
                    loadToL2(addr, 'L');
                }
                loadToL1I(addr);
            }
            break;
            
        case 'L': // 数据读取
            handleLoad(addr);
            break;
            
        case 'S': // 数据存储
            handleStore(addr);
            break;
            
        case 'M': // 数据修改 (L + S)
            handleModify(addr, len);
            break;
            
        default:
            break;
    }
}
