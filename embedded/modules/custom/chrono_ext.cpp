#include <chrono>
#include "chrono_ext.h"

// Convert ChronoTime to unsigned long (milliseconds)
std::chrono::time_point<std::chrono::steady_clock> getChronoTimeAtNow(){
    return std::chrono::steady_clock::now();
}

std::chrono::steady_clock::duration castChronoTimeToDurationAtEpoch(std::chrono::time_point<std::chrono::steady_clock> chronoTime){
    return chronoTime.time_since_epoch();
}

long long castChronoDurationToMillisLongLong(std::chrono::steady_clock::duration chronoTimeAtNow){
    return std::chrono::duration_cast<std::chrono::milliseconds>(chronoTimeAtNow).count();
}

unsigned long castChronoDurationMillisLongLongToUnsignedLong(long long chronoDurationMillisLongLong){
    return  static_cast<unsigned long>(chronoDurationMillisLongLong);
}

// Convert unsigned long (milliseconds) to ChronoTime
long long castChronoDurationMillisUnsignedLongToLongLong(unsigned long chronoDurationMillisUnsignedLong){
    return static_cast<long long>(chronoDurationMillisUnsignedLong);
}

std::chrono::steady_clock::duration revertDurationMillisLongLongToChronoDuration(long long durationMillisLongLong){
    return std::chrono::milliseconds(durationMillisLongLong);
}

std::chrono::time_point<std::chrono::steady_clock> revertChronoDurationToChronoTime(std::chrono::steady_clock::duration chronoDurationSinceEpoch){
    return std::chrono::time_point<std::chrono::steady_clock>(chronoDurationSinceEpoch);
}

