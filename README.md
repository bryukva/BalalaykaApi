# Test task 1
Rest API with two methods (added deletion and get one object as well): 
1. Adding list of objects (called "balalayka" for not to use boring "object")
  from json looking like
  ```
[
	{"1": "value1"},
	{"5": "value2"},
{"10": “value32"},
….
]
```
The values are being sorted (by code) and the table is being cleared before insertion of the new values.

2. Getting all the values from the table. Additional filters are available: you can choose objects with code that is more or less than indicated boundaries, or filter values that starts or ends with some mask.

I've tried doing it using Minumum API.
The objects in database is being created via ef core migrations, so the structure of the table can be seen here https://github.com/bryukva/BalalaykaApi/blob/main/Balalayka.Data/Models/BalalaykaEntity.cs

The client side is poor because I'm mostly a backend developer, but I could improve it if neccessary (and there's a button redirecting to the standard swagger page if your eyes start bleeding).

# Test task 2

Answer:
```
--1
select	cl.ClientName
	,	count(co.Id) as CountContacts
from	Clients as cl
		join ClientContacts as co on cl.Id = co.ClientId
group by cl.ClientName

--2
select	cl.Id
	,	cl.ClientName
from	Clients as cl
		join ClientContacts as co on cl.Id = co.ClientId
group by cl.Id, cl.ClientName
having count(co.Id) > 2
```
# Test task 3

Answer (Postgre):
```
select	a.Id
	,	a.dt as Sd
	,	c.md as Ed
from	dates as a
		left join lateral (	select	min(dt) as md
						   	from	dates as b
						   	where b.id = a.id and b.dt > a.dt ) as c on true
where c.md is not null
```

In ms sql it should be like this, but I don't have it at hand to verify:
```
select	a.Id
	,	a.dt as Sd
	,	c.md as Ed
from	dates as a
		outer apply (	select	min(dt) as md
						from	dates as b
						where b.id = a.id and b.dt > a.dt ) as c
where c.md is not null
```
