# CoffeeScript

$ ->
  console.log "aaa"
  initDataTables()
  
initDataTables = () ->
  #$("#mata").DataTable()
  #$(".results-grid").DataTable()
  # Convert dates from absolute time to distance from now
  convertDates()
  
# TODO export this into a separate script
convertDates = () ->
  #map `moment` format
  for elem in $(".date.difference")
    convertDate $ elem
    
convertDate = ($elem) ->
  date = $elem.attr("abs-date")
  distance = moment(date, "DD-MMM-YY HH:mm:ss").fromNow()
  console.log "#{date} is #{distance}"
  $elem.text(distance) unless distance is "Invalid date"