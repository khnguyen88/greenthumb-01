#ifndef CHRONO_EXT_H
#define CHRONO_EXT_H

#include <chrono>

// Chronos Ext Method
std::chrono::time_point<std::chrono::steady_clock> getChronoTimeAtNow();

std::chrono::steady_clock::duration castChronoTimeToDurationAtEpoch(std::chrono::time_point<std::chrono::steady_clock> chronoTime);
long long castChronoDurationToMillisLongLong(std::chrono::steady_clock::duration chronoTimeAtNow);
unsigned long castChronoDurationMillisLongLongToUnsignedLong(long long chronoDurationMillisLongLong);

long long castChronoDurationMillisUnsignedLongToLongLong(unsigned long chronoDurationMillisUnsignedLong);
std::chrono::steady_clock::duration revertDurationMillisLongLongToChronoDuration(long long durationMillisLongLong);
std::chrono::time_point<std::chrono::steady_clock> revertChronoDurationToChronoTime(std::chrono::steady_clock::duration chronoDurationSinceEpoch);

#endif
