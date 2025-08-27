# DigiStore


DigiStore is an **e-commerce platform** similar to **Digikala**.  
Developed using **ASP.NET Core** and **Entity Framework**, following the **Repository Pattern**.

I am passionate about **learning and teaching**.  
This project showcases my skills and experience for my portfolio, but anyone can use it for their own projects.

Unlike a regular online store, DigiStore is a **marketplace platform**.  
Anyone can become a seller by verifying their identity (explained below).

---

## 📂 Project Structure

| Layer | Description |
|-------|-------------|
| **DigiStore** | Main project containing **Views**, **Controllers**, and **Layouts** |
| **DigiStore.Core** | Contains **Classes**, **Interfaces**, **Services**, and **ViewModels**.<br>Utilities include password hashing, email/SMS sending, number converters, and layout scopes |
| **DigiStore.DataAccessLayer** | Interacts directly with the database.<br>Set this as **Default Project** when performing migrations |

**Repository Pattern Example:**

IStore → StoreService
IAdmin → AdminService
ITemp → TempService

---

## ⚙️ Getting Started

### Required NuGet Packages
Install the following packages before running the project:

- `Kavenegar.DotNetCore` (for SMS)
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `X.PagedList.Mvc.Core`

### Initial Setup
When the site runs for the first time:

- Creates **Roles**: Admin, Store, User
- Updates the **Settings** table (editable via admin panel)
- Creates a default admin user:

Mobile: 123456
Password: 1234


### Database Migration
If the database is not yet created, run the following commands in **Package Manager Console** (Default Project: `DigiStore.DataAccessLayer`):


- `Enable-Migrations`
- `Add-Migration Mig-DigiStore`
- `Update-Database`


## 🎨 Design & Layout

- **Admin panel & authentication pages:** Bootstrap 4 + CSS  
- **Main site:** Custom CSS only  
- All category data is read from the database  
- Homepage connects slides, banners, and discounted products dynamically  
- Designed to scale like large marketplaces (Digikala)  

---

## 🛒 Features

- Sellers can register via **"Become a Seller"** option  
- Identity verification via **SMS and Email**  
- Sellers select their category and submit identification documents for admin approval  
- Both admins and sellers can create brands (**admin approval required**)  
- Ready-to-use banners & advertisements  
- Admin panel supports showcasing:
  - Newest products
  - Most viewed products
  - Popular brands
  - Latest articles  

---

## 🔧 Workflow

1. User registers as a seller via **User Panel → Become a Seller**  
2. Verifies account via **SMS/Email**  
3. Selects a category and submits identification documents  
4. Admin approves seller  
5. Seller can now add products  
6. Brand creation requires admin approval  

---

## 🏆 Project Highlights

- Clean and maintainable code structure  
- Fully layered architecture for scalability  
- Marketplace functionality built on top of a standard e-commerce framework  
- Ready for enhancements like:
  - Seller stories
  - Product promotions
  - Advanced analytics  

---

## 📌 Notes

- Admin panel & pages are **Bootstrap 4**  
- Main site uses **pure CSS**  
- Database must be properly migrated before first run  
- Categories, subcategories, and sub-subcategories must be registered in admin panel  
- Homepage only displays slides, banners, and discounted products dynamically  
- The project is designed to be **extendable and maintainable**  

---

## 📚 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 🔗 Contact

- **Developer:** [Alireza Mohammadi]  
- **Email:** alirezaamohammadiii73@gmail.com  
- **Linkedin.:** [https://www.linkedin.com/in/alireza-mohammadi-599924139]  
