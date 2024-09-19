create database ManagerLibrary


CREATE TABLE [admins] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] nvarchar(255) NOT NULL,
  [admin_name] nvarchar(255) NOT NULL,
  [password] nvarchar(500) NOT NULL,
  [created_at] datetime NOT NULL,
  [updated_at] datetime NOT NULL,
  [delete_flag] bit NOT NULL
)
GO

CREATE TABLE [authors] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] nvarchar(255) NOT NULL,
  [created_at] datetime NOT NULL,
  [updated_at] datetime NOT NULL,
  [delete_flag] bit NOT NULL
)
GO

CREATE TABLE [genres] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] nvarchar(255) NOT NULL,
  [created_at] datetime NOT NULL,
  [updated_at] datetime NOT NULL,
  [delete_flag] bit NOT NULL
)
GO

CREATE TABLE [books] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [title] nvarchar(255) NOT NULL,
  [image] nvarchar(500),
  [subtitle] nvarchar(500),
  [author_id] int,
  [genre_id] int,
  [publishing_year] int,
  [quantity_in_stock] int NOT NULL,
  [description] nvarchar(1000),
  [created_at] datetime NOT NULL,
  [updated_at] datetime NOT NULL,
  [delete_flag] bit NOT NULL
)
GO

CREATE TABLE [users] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [name] nvarchar(255) NOT NULL,
  [user_name] nvarchar(255) NOT NULL,
  [password] nvarchar(500) NOT NULL,
  [created_at] datetime NOT NULL,
  [updated_at] datetime NOT NULL,
  [delete_flag] bit NOT NULL
)
GO

CREATE TABLE [borrowings] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [user_id] int NOT NULL,
  [start_at] datetime NOT NULL,
  [end_at] datetime NOT NULL,
  [actual_end_at] datetime,
  [created_at] datetime NOT NULL,
  [updated_at] datetime NOT NULL,
  [delete_flag] bit NOT NULL
)
GO

CREATE TABLE [borrowing_items] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [borrowing_id] int NOT NULL,
  [book_id] int NOT NULL,
  [quantity] int NOT NULL,
  [created_at] datetime NOT NULL,
  [updated_at] datetime NOT NULL,
  [delete_flag] bit NOT NULL
)
GO

CREATE TABLE [ratings] (
  [id] int PRIMARY KEY NOT NULL IDENTITY(1, 1),
  [book_id] int NOT NULL,
  [user_id] int NOT NULL,
  [star] int NOT NULL,
  [created_at] datetime NOT NULL,
  [updated_at] datetime NOT NULL,
  [delete_flag] bit NOT NULL
)
GO

ALTER TABLE [books] ADD FOREIGN KEY ([author_id]) REFERENCES [authors] ([id])
GO

ALTER TABLE [books] ADD FOREIGN KEY ([genre_id]) REFERENCES [genres] ([id])
GO

ALTER TABLE [borrowings] ADD FOREIGN KEY ([user_id]) REFERENCES [users] ([id])
GO

ALTER TABLE [borrowing_items] ADD FOREIGN KEY ([borrowing_id]) REFERENCES [borrowings] ([id])
GO

ALTER TABLE [borrowing_items] ADD FOREIGN KEY ([book_id]) REFERENCES [books] ([id])
GO

ALTER TABLE [ratings] ADD FOREIGN KEY ([book_id]) REFERENCES [books] ([id])
GO

ALTER TABLE [ratings] ADD FOREIGN KEY ([user_id]) REFERENCES [users] ([id])
GO


 -- data
INSERT INTO [admins] (name, admin_name, password, created_at, updated_at, delete_flag) VALUES
('Alice Johnson', 'admin1', '1', GETDATE(), GETDATE(), 0);

-- Insert data into users
INSERT INTO [users] (name, user_name, password, created_at, updated_at, delete_flag) VALUES
('John Doe', 'user1', '1', GETDATE(), GETDATE(), 0),
('Michael Johnson', 'user2', '1', GETDATE(), GETDATE(), 0);

INSERT INTO [authors] (name, created_at, updated_at, delete_flag) VALUES
('J.K. Rowling', GETDATE(), GETDATE(), 0),
('George R.R. Martin', GETDATE(), GETDATE(), 0),
('J.R.R. Tolkien', GETDATE(), GETDATE(), 0),
('Agatha Christie', GETDATE(), GETDATE(), 0),
('Isaac Asimov', GETDATE(), GETDATE(), 0);

INSERT INTO [genres] (name, created_at, updated_at, delete_flag) VALUES
('Fiction', GETDATE(), GETDATE(), 0),
('Non-Fiction', GETDATE(), GETDATE(), 0),
('Fantasy', GETDATE(), GETDATE(), 0),
('Science', GETDATE(), GETDATE(), 0),
('History', GETDATE(), GETDATE(), 0);

INSERT INTO [books] (title, image, subtitle, author_id, genre_id, publishing_year, quantity_in_stock, description, created_at, updated_at, delete_flag) VALUES
('The Great Adventure', 'https://picsum.photos/400/300?random=1', 'A Journey Beyond', 1, 5, 2021, 5, 'A thrilling adventure across uncharted territories.', GETDATE(), GETDATE(), 0),
('Science of the Universe', 'https://picsum.photos/400/300?random=2', 'Exploring the Cosmos', 2, 4, 2020, 10, 'An in-depth look at the mysteries of the cosmos and space science.', GETDATE(), GETDATE(), 0),
('Mystical Fantasy', 'https://picsum.photos/400/300?random=3', 'The World of Magic', 3, 2, 2019, 7, 'Dive into a realm of magic, mystery, and fantastic creatures.', GETDATE(), GETDATE(), 0),
('Innovations in Science', 'https://picsum.photos/400/300?random=4', 'New Discoveries', 4, 3, 2018, 3, 'A detailed account of recent innovations and breakthroughs in science.', GETDATE(), GETDATE(), 0),
('Historical Events', 'https://picsum.photos/400/300?random=5', 'Moments that Shaped the World', 5, 1, 2017, 6, 'A comprehensive guide to the most significant events in world history.', GETDATE(), GETDATE(), 0),
('The Silent Forest', 'https://picsum.photos/400/300?random=6', 'Secrets of the Wilderness', 1, 2, 2022, 8, 'An intriguing exploration of the hidden wonders and mysteries of forests.', GETDATE(), GETDATE(), 0),
('Digital Frontiers', 'https://picsum.photos/400/300?random=7', 'Technology and Innovation', 2, 3, 2021, 12, 'A comprehensive look at the latest advancements in digital technology.', GETDATE(), GETDATE(), 0),
('The Lost Kingdom', 'https://picsum.photos/400/300?random=8', 'Epic Tales of Ancient Times', 3, 4, 2020, 9, 'A captivating story set in a mythical kingdom filled with adventure and legend.', GETDATE(), GETDATE(), 0),
('Future Visions', 'https://picsum.photos/400/300?random=9', 'Predictions and Possibilities', 4, 5, 2019, 15, 'An insightful analysis of future trends and what they mean for society.', GETDATE(), GETDATE(), 0),
('Art Through Ages', 'https://picsum.photos/400/300?random=10', 'A Journey Through Art History', 5, 1, 2018, 11, 'An exploration of art history from ancient times to the modern era.', GETDATE(), GETDATE(), 0);

INSERT INTO [borrowings] (user_id, start_at, end_at, actual_end_at, created_at, updated_at, delete_flag) VALUES
(1, GETDATE(), DATEADD(day, 10, GETDATE()), NULL, GETDATE(), GETDATE(), 0),
(1, GETDATE(), DATEADD(day, 10, GETDATE()), NULL, GETDATE(), GETDATE(), 0),
(2, GETDATE(), DATEADD(day, 10, GETDATE()), NULL, GETDATE(), GETDATE(), 0),
(2, GETDATE(), DATEADD(day, 10, GETDATE()), NULL, GETDATE(), GETDATE(), 0),
(1, GETDATE(), DATEADD(day, 10, GETDATE()), NULL, GETDATE(), GETDATE(), 0);

INSERT INTO [borrowing_items] (borrowing_id, book_id, quantity, created_at, updated_at, delete_flag) VALUES
(1, 1, 1, GETDATE(), GETDATE(), 0),
(2, 2, 1, GETDATE(), GETDATE(), 0),
(3, 3, 1, GETDATE(), GETDATE(), 0),
(4, 4, 1, GETDATE(), GETDATE(), 0),
(5, 5, 1, GETDATE(), GETDATE(), 0);

INSERT INTO [ratings] (book_id, user_id, star, created_at, updated_at, delete_flag) VALUES
(1, 1, 5, GETDATE(), GETDATE(), 0),
(2, 2, 4, GETDATE(), GETDATE(), 0),
(3, 2, 3, GETDATE(), GETDATE(), 0),
(4, 1, 5, GETDATE(), GETDATE(), 0),
(5, 1, 4, GETDATE(), GETDATE(), 0);
