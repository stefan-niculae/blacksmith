# CoffeeScript

$ ->
  for article in $ "#submitted article"
    prepareArticle $ article
    
  # Refresh updating date difference every minute
  setInterval convertDates, 59000

prepareArticle = (article) ->
  convertDate article.find ".date.difference"
  for editable in article.find "[allows-edit], .category[contenteditable]"
    rememberInitialValue $ editable

rememberInitialValue = ($elem) ->
  # Save original value for undoing 
  # and the one the db knows to save unnecessary DB accesses
  value = $elem.text().trim()
  $elem.data("original", value)
  $elem.data("db-cache", value)
  
@focused

@registerFocus = (id, type) ->
  @focused = { id: id, type: type }
  
@updateFocused = () ->
  id = @focused.id;
  type = @focused.type
  
  if type not in ["title", "address", "description", "category"]
    console.error "Invalid focused type: #{type}"
    return
    
  elem =  $ "[db-id='#{id}']"
  # HACK
  field = if type isnt "category" then elem.find ".#{type}" else $ ".category"
  content = field.text().trim()
  indicator = elem.find '.working-indicator'
  
  lastContent = field.data "last-content"
  
  # No need to continue if it is at the value the db knows
  if content is field.data "db-cache"
    clearTimeout field.data("update-timeout") unless not field.data "update-timeout"
    indicator.css "visibility", "hidden"
    return
  
  # Don't continue if the keypress/deselect changed nothing
  if lastContent? and lastContent is content
    return
  field.data "last-content", content
  
  # When updating the address, also update the favicon and the link
  if type is "address"
    elem.find ".favicon"
      .attr "src", "http://www.google.com/s2/favicons?domain_url=#{content}"
    elem
      .find ".address a"
      .attr "href", "#{prependHttp content}"
      
  # Show the working indicator
  indicator.css "visibility", "visible" 
  
  # Update the date
  if type isnt "category"
    date = elem.find ".date.difference"
    formattedDate = moment().format "DD-MMM-YY HH:mm:ss"
    date
      .attr "abs-date", formattedDate
      .attr "title", "Date updated: #{formattedDate}"
  
  # Give the user a 1sec period from keystroke to keystroke before
  # declaring the input finished and sending the update to the DB
  clearTimeout field.data("update-timeout") unless not field.data "update-timeout"
  f = () -> updateDB type, content, id, indicator, date, field
  field.data "update-timeout", setTimeout f, 1000

@prependHttp = (link) ->
  toPrepend = "http://"
  start = link[...toPrepend.length]
  
  # Check if it doesn't already contain the http
  if start isnt toPrepend
    return toPrepend + link
  return link
  
@updateDB = (type, value, id, indicator, date, field) ->
  # TODO have a better way of inserting params into url, rather than hardcoding it this way
  # Send DB update parameters
  # FIXME don't redirect to window.location.href, instead to link directly or something!
  $.ajax url: "#{window.location.href}&Action=Update&field=#{type}&value=#{value}&id=#{id}"
    .done ->
      # Update the new DB known value
      field.data "db-cache", value
      # Remove the pending update indicator
      indicator.css "visibility", "hidden"
      # Update the date distance
      if type isnt "category"
        convertDate date
      console.log "done updating! #{type} to #{value}"
      
@sendDelete = () ->
  article = $(event.currentTarget).parent "article"
  id = article.attr "db-id"
  $.ajax url: "#{window.location.href}&Action=Delete&id=#{id}"
    .done ->
      article.slideUp()
      console.log "done deleting!"
      
@toggleEditable = () ->
  article = $(event.currentTarget).parent "article"
  
  editable = article.find "[allows-edit][contenteditable]"
  notEditable = article.find "[allows-edit]:not([contenteditable])"
  
  editable.removeAttr "contenteditable" # make them no longer editable
  notEditable.attr "contenteditable", "" # make them editable