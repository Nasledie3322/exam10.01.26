// public async Task<Response<string>> AddAsync(BookDto bookDto)
//     {
//         _logger.LogInformation("Starting the process of adding book");
//         var book = new Book()
//         { 
//             Title = bookDto.Title,
//             PublishedYear = bookDto.PublishedYear,
//             Genre = bookDto.Genre,
//             AuthorId = bookDto.AuthorId
//         };
//         try
//         {
//             var conn = context.Connection();
//             var query = "insert into books(title,published_year,genre,author_id) values(@title,@p_y,@genre,@a_id)";
//             var res = await conn.ExecuteAsync(query,new{title = book.Title,p_y=book.PublishedYear,genre = book.Genre,a_id = book.AuthorId});
//             if (res == 0)
//             {
//                 _logger.LogWarning("In the process of adding book something went wrong and book wasn't added");
//                 return new Response<string>(HttpStatusCode.InternalServerError, "Book not added");
//             }
//             else kkd
//             {
//                 _logger.LogInformation("While adding book nothing went wrong");
//                 return new Response<string>(HttpStatusCode.OK, "Book was added successfully");
//             }
//         }
//         catch(Exception ex)
//         {
//             _logger.LogWarning(ex.Message);
//             return new Response<string>(HttpStatusCode.InternalServerError, "Internal Server Error");
//         }
//     }