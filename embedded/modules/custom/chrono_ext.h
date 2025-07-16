#ifndef CHRONO_EXT_H
#define CHRONO_EXT_H

#include <chrono>

// Chronos Ext Method
std::chrono::time_point<std::chrono::steady_clock> getChronoTimeAtNow();
std::chrono::steady_clock::duration getChronoDurationSinceEpoch(std::chrono::time_point<std::chrono::steady_clock> chronoTime);
long long getChronoDurationSinceEpochInMillisLongLong(std::chrono::steady_clock::duration chronoTimeAtNow);
unsigned long getChronoDurationSinceEpochInMillisUnsignedLong(long long chronoDurationMillisLongLong);

#endif
