# EntitySqlBuilder
Building SQL statements for Entities by using Types and Expressions instead of just strings.

By using Linq Expression on classes, SQL statemts like<br>
`select Firstname, Lastname from Customer`

can be build as<br>
`
var builder = new SqlBuilder<Customer>();
var sql = builder.Select.Columns(c=>c.Firstname, c=>c.Lastname).From().ToString();
`<br>
or<br>
`SELECT * FROM Customer WHERE ((Lastname = 'Doe') AND (Firstname = 'John'))`<br>
as<br>
`
var builder = new SqlBuilder<Customer>();
var sql = builder.Select.All().From()
						.Where.Expr(c => c.Lastname == "Doe" && c.Firstname == "John")
						.ToString();
`

I used EntitySqlBuiler together with Dapper, saving time and frustrations hunting typos at runtime and ging the advantage, easily changes column names from SQL tables.

