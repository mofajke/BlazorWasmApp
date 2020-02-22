select
	id,
	username,
	email
from public.Users
where username = @login and password = @password
fetch first 1 rows only;