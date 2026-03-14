-- 31. Text search type
create table ts_example (
	id bigint generated always as identity primary key,
	content text,
	search_verctor_en tsvector generated always as (to_tsvector('english', content)) stored
)
insert into ts_example (content) values ('The quick brown fox jumps over the lazy dog')
insert into ts_example (content) values ('The quick brown fox jumps over the cat')
select * from ts_example where search_verctor_en @@ to_tsquery('cat')


-- 32. Bit string
select b'0101' & B'0001'  -- bit mask
create table bits_example (
	bit3 bit(3),
	bitv bit varying(32)
)
insert into bits_example (bit3, bitv) values ('011', '01010100011101')
insert into bits_example (bit3, bitv) values (b'101', b'0101010001')
select * from bits_example


-- 33. Ranges
select '[1, 5]'::int4range   -- [1, 6) 6 exclusive
-- int range has discrete steps
-- 1, 2, 3, 4, 5
-- date range has discrete steps, day by day

select '[1, 5]'::numrange  -- [1, 5] 5 inclusive
-- num range is continuous, no discrete step
-- the number is infinity, like:
-- 1, 1.0000000000001, 1.000000000002 ...
-- timestamp functionally the same thing

select numrange(1, 5, '(]');

CREATE TABLE range_example (
	id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	int_range INT4RANGE,
	num_range NUMRANGE,
	date_range DATERANGE,
	ts_range TSRANGE
)

INSERT INTO range_example (int_range, num_range, date_range, ts_range)
VALUES
('[1,11)'::int4range, '[0.5,5.5)'::numrange, '[2023-01-01,2024-01-01)'::daterange, '["2023-09-01 00:00:00","2023-09-30 23:59:00"]'::tsrange),
('[2,101)'::int4range, '(0.0,10.0]'::numrange, '[2022-01-01,2022-06-01)'::daterange, '("2023-01-01 00:00:00","2023-01-10 12:00:00"]'::tsrange),
('[10,20)'::int4range, '[1.0,2.0)'::numrange, 'empty'::daterange, 'empty'::tsrange),
('[5,)'::int4range, '[1.0,)'::numrange, '(,)'::daterange, '(,"2023-01-01 00:00:00")'::tsrange);
select * from range_example

-- '@>' check if a range contains a value
select * from range_example where int_range @> 5

-- check if two range overlap, '&&'
select * from range_example where int_range && '[20, 22]'

-- intersection of 2 ranges
select int4range(10, 20) * int4range(15, 25)
select int4range(10, 20, '[]') * int4range(15, 25)
-- inclusive, exclusive, continous range, discrete range

-- upper / lower
select upper(int4range(10, 20, '[]')), upper_inc(int4range(10, 20, '[]'))

-- multi-range
select '{[3, 7), [8, 9)}'::int4multirange @> 7


-- 34. Composite types
create type address as (
	number text,
	street text, 
	city text,
	state text,
	postal text
);
select row('123', 'Main St', 'Anytown', 'ST', '12345')::address    -- can do without 'row'

create table addresses (
	id bigint generated always as identity primary key,
	addr address
)
insert into addresses (addr) values(('123', 'Main St', 'Anytown', 'ST', '12345'))

select id, (addr).street from addresses -- paranthesis is must


-- 35. Nulls
-- you will get a lot good thing from restrain the column not nullable, like indexing, grouping, comparing, sorting etc.
create table products (
	id bigint generated always as identity primary key,
	name text not null,
	price numeric not null check(price > 0)
)
insert into products (name, price) values ('chest', '20.4234')
insert into products (name, price) values ('majiang', 0)
select * from products
drop table products


-- 36. Unique constraints
-- Primary key auto add not null and unique
-- can insert null into uqniue column
-- allow null but not distinct, you only allow 1 null in the column
-- Unique can be used for the combination of the columns, used as talbe constraint


-- 37. Exclusion constraint
-- GIST: a type of index
-- && is overlap check, if it's overlap then excluded
CREATE TABLE reservations (
	id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	room_id INTEGER,
	reservation_period TSRANGE,
	EXCLUDE USING GIST (reservation_period WITH &&)
)

INSERT INTO reservations (room_id, reservation_period) VALUES (1, '[2023-09-01 14:00, 2023-09-03 12:00)');
select * from reservations

-- but if someone want to book to another room, it still cannot book, since the overlap exclusion constrain
INSERT INTO reservations (room_id, reservation_period) VALUES (2, '[2023-09-02 14:00, 2023-09-04 12:00)');

drop table reservations
-- we need to check the room in the exclusion constrain as well 
CREATE TABLE reservations (
	id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	room_id INTEGER,
	reservation_period TSRANGE,
	EXCLUDE USING GIST (room_id WITH =, reservation_period WITH &&)
);

-- but the gist doesn't have the default = operation for integer, we need to enable the btree_gist
-- after enable the btree_gist, we can run the create table again.
CREATE EXTENSION IF NOT EXISTS btree_gist;

INSERT INTO reservations (room_id, reservation_period) VALUES (1, '[2023-09-01 14:00, 2023-09-03 12:00)');
INSERT INTO reservations (room_id, reservation_period) VALUES (2, '[2023-09-02 14:00, 2023-09-04 12:00)');
INSERT INTO reservations (room_id, reservation_period) VALUES (2, '[2023-09-01 14:00, 2023-09-04 12:00)');


-- Then what if the reservation being cancled? we add a enum in the table, here using text instead, no time to do that :)
drop table reservations;
create table reservations (
	id bigint generated always as identity primary key,
	room_id integer,
	booking_status text,
	reservation_period tsrange,
	exclude using gist (room_id with =, reservation_period with &&)
);

insert into reservations (room_id, booking_status, reservation_period) 
values (1, 'canceled', '[2023-09-01 14:00, 2023-09-03 12:00)');
select * from reservations

-- based on the previous definition, we can't insert a confrimed book record, even the previous one canceled
insert into reservations (room_id, booking_status, reservation_period) 
values (1, 'confirmed', '[2023-09-01 14:00, 2023-09-03 12:00)');

-- we need to re-define the table, add the where constraint to the exclude
drop table reservations;
create table reservations (
	id bigint generated always as identity primary key,
	room_id integer,
	booking_status text,
	reservation_period tsrange,
	exclude using gist (room_id with =, reservation_period with &&) where (booking_status != 'canceled')
);

select * from reservations

-- then try to insert two overlapped records with different booking_status, can only confirm once.
insert into reservations (room_id, booking_status, reservation_period) 
values (1, 'canceled', '[2023-09-01 14:00, 2023-09-03 12:00)');
insert into reservations (room_id, booking_status, reservation_period) 
values (1, 'confirmed', '[2023-09-01 14:00, 2023-09-03 12:00)');


-- 38. Foreign-key constraint
create table states (
	id bigint generated always as identity primary key,
	name text
);
create table cities (
	id bigint generated always as identity primary key,
	state_id bigint references states(id),
	name text
);
insert into states (name) values ('Shaanxi');
select * from states;
insert into cities (state_id, name) values (1, 'xi''an')
select * from cities

-- Or you can define the foreign key on table level,
-- this can also used for composite foreign key
drop table cities;
create table cities (
	id bigint generated always as identity primary key,
	state_id bigint, 
	name text,
	foreign key (state_id) references states(id)   -- on delete no action / restrict / cascade
);
insert into cities(state_id, name) values (1, 'xi''an');

-- difference between no action / restrict? 
	-- no action allow check deferred to later transaction


-- 39. Introduction to indexes
	-- When creating a index for a column, 
	-- it's actually create a separate index table, which including
		-- that column data
		-- and a pointer to point to the physical address of that record
	-- normally it uses b-tree to create for easy traverse and fast lookup
	
	-- So it's better to not create index for every columns of a table, slow down the database
		-- When update the table, the index needs to update as well.


-- 40. Heaps and CTIDs
	-- index contains a pointer point back to the table.
	-- How does postgresql store rows under the hood, how does it write the data to disk?
		-- postgres has a bunch of pages (eaqual size blocks), in the page, there are rows
		-- so, you can easily get the data from page 5 row 8
	
	-- postgres put the data in whereever it can, 
		-- like page5 row8, page10 row2, 
		-- it's a heap, fast to store and lookup
		
select *, ctid from reservations -- where ctid = '(0,4)' 
-- ctid (0, 4), means data in page 0, position 4
-- you can use a where clause for ctid, HOWEVER DON'T, these ctids will change after vaccum the database.

-- Every index contains the ctids, that's the actually pointer to get you back to the table to find the data.
 













