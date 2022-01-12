# EfCoreHelper

### Короткий опис
Після того як заюзав db first (ef core) запусти цю прогу для створення конфігураційних класів. Також всі ентіті моделі будуть перетворені в `record`

### Важливі моменти
- Ентіті моделі мають лежати в окремій папці `Models` або `Entities`
- Ім'я контексту має обов'язково закінчуватись словом **Context**, типу `YourNameContext`

### Шаблон CLI команди 
```cli
dotnet ef dbcontext scaffold "Server=YOUR_SERVER;Database=DB_NAME;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -c YOUR_NAMEContext --context-dir Contexts --output-dir Models

```
