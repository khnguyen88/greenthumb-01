#include <chrono>
#include "chrono_ext.h"

std::chrono::time_point<std::chrono::steady_clock> getChronoTimeAtNow(){
    return std::chrono::steady_clock::now();
}

std::chrono::steady_clock::duration getChronoDurationSinceEpoch(std::chrono::time_point<std::chrono::steady_clock> chronoTime){
    return chronoTime.time_since_epoch();
}

long long getChronoDurationSinceEpochInMillisLongLong(std::chrono::steady_clock::duration chronoTimeAtNow){
    return std::chrono::duration_cast<std::chrono::milliseconds>(chronoTimeAtNow).count();
}

unsigned long getChronoDurationSinceEpochInMillisUnsignedLong(long long chronoDurationMillisTrunc){
    return  static_cast<unsigned long>(chronoDurationMillisTrunc) * 1;
}

