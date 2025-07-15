#include "time_unit_converter.h"

long secondsToMillis(long seconds){
  return seconds * 1000;
}

long minutesToMillis(long minutes){
  return minutes * 60 * 1000;
}

long hoursToMillis(long hours){
  return hours * 60 * 60 * 1000;
}

long millisToSeconds(long millis){
  return millis / 1000;
}

long millisToMinutes(long millis){
  return millis / (60 * 1000);
}

long millisToHours(long millis){
  return millis / (60 * 60 * 1000);
}