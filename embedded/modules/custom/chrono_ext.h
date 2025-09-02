#ifndef CHRONO_EXT_H
#define CHRONO_EXT_H

#define _CHRONO_TP std::chrono::time_point
#define _CHRONO_SC_D std::chrono::steady_clock::duration

#include <chrono>

// Chronos Ext Method
_CHRONO_TP <std::chrono::steady_clock> getChronoTimeAtNow();

_CHRONO_SC_D castChronoTimeToDurationAtEpoch(std::chrono::time_point<std::chrono::steady_clock> chronoTime);
long long castChronoDurationToMillisLongLong(std::chrono::steady_clock::duration chronoTimeAtNow);
unsigned long castChronoDurationMillisLongLongToUnsignedLong(long long chronoDurationMillisLongLong);

long long castChronoDurationMillisUnsignedLongToLongLong(unsigned long chronoDurationMillisUnsignedLong);
_CHRONO_SC_D revertDurationMillisLongLongToChronoDuration(long long durationMillisLongLong);
_CHRONO_TP <std::chrono::steady_clock> revertChronoDurationToChronoTime(std::chrono::steady_clock::duration chronoDurationSinceEpoch);

#endif
