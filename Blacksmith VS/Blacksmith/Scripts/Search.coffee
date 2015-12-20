# CoffeeScript

$ ->
  console.log "aaa"
  initDataTables()
  
initDataTables = () ->
  $("#mata").DataTable()
  #$(".results-grid").DataTable()