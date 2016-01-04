# CoffeeScript

@sendInsert = () ->
  form = $ "#creation"
  title = form.find(".title").text().trim()
  address = form.find(".address").text().trim()
  description = form.find(".description").text().trim()
  
  console.log "url = #{window.location.href}&Action=Insert&title=#{title}&address=#{address}&description=#{description}"
  
  $ "<div></div>"
    .addClass "just-inserted"
    .css "display", "none"
    .insertAfter "#submitted h2"
    .load "#{window.location.href}&Action=Insert&title=#{title}&address=#{address}&description=#{description} #submitted article:first",  -> 
      $ this
        .slideDown
          complete: ->
            $ this
              .find "article"
              .unwrap()
              
      # The UpdateDelete script is required in order to display the added one
      prepareArticle $ this
      console.log "done inserting and loading!"
