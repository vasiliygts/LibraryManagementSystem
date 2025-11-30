# LibraryManagementSystem — інформаційна система для управління шкільними бібліотеками

Настільний WPF-застосунок для автоматизації роботи шкільної бібліотеки: облік книжкового фонду та читачів, реєстрація видачі й повернення книг, пошук та експорт списків у файл.

Проєкт створено в рамках практичного завдання **«Інформаційна система для управління шкільними бібліотеками»**.

---

## Основний функціонал

- **Книги**
  - Перегляд списку книг у таблиці.
  - Додавання / редагування / видалення книг.
  - Зберігання основної інформації: назва, автор, жанр, рік видання, ISBN, доступність.
  - Експорт списку книг (або відфільтрованого) у текстовий файл.

- **Читачі**
  - Облік читачів (учні, вчителі).
  - Додавання / редагування / видалення читачів.
  - Перевірка обов’язкових полів та базова валідація email.

- **Видача та повернення книг**
  - Створення операцій видачі/повернення: вибір книги та читача, дати видачі й повернення.
  - Перевірка коректності дат (повернення не раніше видачі).
  - Контроль, щоб книга не могла бути видана повторно, поки не повернута.

- **Пошук та фільтрація**
  - Текстовий пошук по кількох полях одночасно (назва, автор, жанр, ISBN для книг; ПІБ, клас, телефон, email для читачів).
  - Миттєва фільтрація списку під час введення тексту.

---

## Технології

- **Мова програмування:** C#
- **Платформа:** .NET 6+ (WPF, цільова платформа `net6.0-windows`)
- **UI-фреймворк:** Windows Presentation Foundation (WPF)
- **Робота з БД:** Entity Framework Core + провайдер **Npgsql**
- **СУБД:** PostgreSQL
- **IDE:** Microsoft Visual Studio 2022 (або новіша)

---

## Структура проєкту (коротко)

- `App.xaml`, `App.xaml.cs` – запуск застосунку.
- `MainWindow.xaml` – головне вікно / меню навігації між модулями.
- `Data/LibraryContext.cs` – DbContext для доступу до PostgreSQL.
- `Models/` – моделі доменних сутностей (книги, читачі, операції видачі/повернення).
- `BooksWindow.xaml` (+ дочірні вікна) – робота з книгами.
- `ReadersWindow.xaml` – робота з читачами.
- `IssueReturnWindow.xaml` (назва може відрізнятись) – форма видачі та повернення книг.

---

## Як запустити

### 1. Необхідні інструменти

- **ОС:** Windows 10 / 11  
- **.NET SDK:** 6.0 або новіший (із підтримкою WPF)  
- **PostgreSQL:** встановлена локально або на сервері  
- **IDE:** Visual Studio 2022 із робочим навантаженням *.NET Desktop Development*

---

### 2. Клонувати репозиторій

~~~bash
git clone https://github.com/<your-username>/<your-repo-name>.git
cd <your-repo-name>
~~~

---

### 3. Створити базу даних у PostgreSQL

Підключіться до PostgreSQL (через `psql`, DBeaver, pgAdmin тощо) та створіть базу:

~~~sql
CREATE DATABASE school_library;
~~~

Якщо в репозиторії є папка `sql/` із скриптами – виконайте їх у цій базі, щоб створити таблиці та, за потреби, початкові дані.

---

### 4. Налаштувати рядок підключення

Рядок підключення зберігається у `LibraryContext` (метод `OnConfiguring`) або в конфігураційному файлі.

~~~csharp
optionsBuilder.UseNpgsql(
    "Host=localhost;Port=5432;Database=school_library;Username=postgres;Password=your_password");
~~~

Замініть `Host`, `Port`, `Database`, `Username`, `Password` на свої значення.

---

### 5. Запуск через Visual Studio

1. Відкрийте файл рішення `.sln` у **Visual Studio**.  
2. Переконайтеся, що стартовим проєктом обрано WPF-застосунок (наприклад, `LibraryManagementSystem`).  
3. Виберіть конфігурацію **Debug**.  
4. Натисніть **F5** або кнопку **Start** для запуску.

---

### 6. Запуск через командний рядок (альтернатива)

~~~bash
cd LibraryManagementSystem
dotnet restore
dotnet run
~~~

---

## Скріншоти

<img width="1969" height="1241" alt="image" src="https://github.com/user-attachments/assets/decd30e8-8d98-4c27-b29b-bf03e8270279" />

<img width="1005" height="574" alt="image" src="https://github.com/user-attachments/assets/425bce49-7c95-4164-b4bf-b477a7e9daa1" />

<img width="1008" height="377" alt="image" src="https://github.com/user-attachments/assets/cf003c93-bc3d-4f65-bfa7-db5a2b1f03c4" />

<img width="1005" height="581" alt="image" src="https://github.com/user-attachments/assets/f311c8ba-803f-4d42-8d93-cb9d31cb2b21" />

<img width="1005" height="567" alt="image" src="https://github.com/user-attachments/assets/875e5604-3136-4e0d-8562-8e2230d52fe0" />

<img width="1005" height="562" alt="image" src="https://github.com/user-attachments/assets/014929e3-56dc-46f1-bb81-5d3bd2633c10" />

<img width="1005" height="580" alt="image" src="https://github.com/user-attachments/assets/0e1b45bd-3a73-430d-88d2-8e6d553575b6" />

<img width="1005" height="572" alt="image" src="https://github.com/user-attachments/assets/2c1f126a-9d53-435f-8b32-bef48166a4ab" />

<img width="1006" height="567" alt="image" src="https://github.com/user-attachments/assets/da175b81-5ee0-4cfc-bb6a-dd2b1941646f" />

<img width="1007" height="582" alt="image" src="https://github.com/user-attachments/assets/432130d6-f0ea-4a2d-8e96-9ea026212f9d" />

<img width="1007" height="405" alt="image" src="https://github.com/user-attachments/assets/04f9f816-2865-480e-b3ff-7dd7ab5b6195" />

<img width="1006" height="479" alt="image" src="https://github.com/user-attachments/assets/787a6fbc-534c-4c74-9d37-ad5bdc5a2f89" />

<img width="1006" height="622" alt="image" src="https://github.com/user-attachments/assets/51754a91-136e-42cf-8e9c-308500d88606" />













