# CoffeeScript

$ ->
  convertDates()
  
@convertDates = () ->
  for elem in $ ".date.difference"
    convertDate $ elem
    
  for elem in $ ".date.short-diff"
    convertDateShort $ elem

@convertDate = ($elem) ->
  # Convert dates from absolute time to distance from now
  date = $elem.attr "abs-date"
  distance = moment(date, "DD-MMM-YY HH:mm:ss").fromNow()
  $elem.text(distance) unless distance is "Invalid date"
  
convertDateShort = ($elem) ->
  # 2 days ago -> 2d
  original = $elem.text()
    
  distance = moment(original, "DD-MMM-YY HH:mm:ss").fromNow(true)
  return if distance is "Invalid Date"
    
  shortened = distance
    .replace /\s*a few seconds\s*/i, "now"
    .replace /\s*seconds?\s*/i, "s"
    .replace /\s*minutes?\s*/i, "m"
    .replace /\s*hours?\s*/i,   "h"
    .replace /\s*days?\s*/i,    "d"
    .replace /\s*months?\s*/i,  "mo"
    .replace /\s*years?\s*/i,   "y"
    .replace /\s*a\s*/i,        "1"
      
  $elem.text(shortened)