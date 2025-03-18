-- Create data base
CREATE DATABASE TestDb
    WITH ENCODING = 'UTF8'
         LC_COLLATE = 'en_US.UTF-8'
         LC_CTYPE = 'en_US.UTF-8'
         TEMPLATE = template0;

-- Set Time zone UTC
ALTER DATABASE TestDb SET TIMEZONE = 'UTC';