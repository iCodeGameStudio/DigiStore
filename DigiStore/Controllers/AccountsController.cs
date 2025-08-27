using DigiStore.Core.Classes;
using DigiStore.Core.Interfaces;
using DigiStore.Core.ViewModels;
using DigiStore.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace DigiStore.Controllers
{
    public class AccountsController : Controller
    {
        public IUser _user;
        private IAccount _account;
        private MessageSender _messageSender;
        private IViewRenderService _render;
        private PersianCalendar pc = new PersianCalendar();
        public AccountsController(IAccount account, IViewRenderService render, IUser user, MessageSender messageSender)
        {
            _account = account;
            _render = render;
            _user = user;
            _messageSender = messageSender;
        }

        private bool CheckLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                return true;
            }
            {
                return false;
            }
        }


        public IActionResult Register()
        {

            if (CheckLogin())
            {
                string username = User.Identity.Name;
                string roleName = _user.GetUserRoleName(username);
                if (roleName == "کاربر")
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                else if (roleName == "فروشگاه")
                {
                    return RedirectToAction("Dashboard", "Panel");

                }
                else if (roleName == "مدیر")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            // 1. بررسی اعتبارسنجی سمت سرور
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["ErrorTitle"] = "خطا در ثبت نام";
                TempData["ErrorMessage"] = "اطلاعات وارد شده نامعتبر است. " + string.Join(" ", errors);
                return RedirectToAction("Register");
            }

            // 2. بررسی وجود شماره موبایل
            if (_account.ExistMobileNumber(NumberConvertor.ToEnglishNumber(viewModel.Mobile)))
            {
                TempData["ErrorTitle"] = "خطا در ثبت نام";
                TempData["ErrorMessage"] = "این شماره موبایل قبلاً ثبت نام کرده است.";
                return RedirectToAction("Login", "Accounts"); // ریدایرکت به صفحه ورود
            }

            // 3. ثبت‌نام کاربر جدید
            User user = new User()
            {
                Mobile = NumberConvertor.ToEnglishNumber(viewModel.Mobile),
                ActiveCode = CodeGenerators.ActiveCode(),
                Code = null,
                Date = pc.GetYear(DateTime.Now).ToString("0000") + "/" +
                       pc.GetMonth(DateTime.Now).ToString("00") + "/" +
                       pc.GetDayOfMonth(DateTime.Now).ToString("00"),
                FullName = null,
                IsActive = false,
                Password = HashGenerators.MD5Encoding(viewModel.Password),
                RoleId = _account.GetMaxRole()
            };

            _account.AddUser(user);

            try
            {
                _messageSender.SMS(viewModel.Mobile,
                    "به فروشگاه اینترنتی خوش آمدید" + Environment.NewLine +
                    "کد فعالسازی: " + user.ActiveCode);
            }
            catch (Exception ex)
            {
                TempData["ErrorTitle"] = "خطا در ارسال کد فعالسازی";
                TempData["ErrorMessage"] = $"پیامک ارسال نشد: {ex.Message}";
                return RedirectToAction("Register");
            }

            // 4. پیام موفقیت و هدایت به صفحه فعالسازی
            TempData["SuccessTitle"] = "ثبت نام موفق";
            TempData["SuccessMessage"] = "کاربر گرامی، ثبت نام با موفقیت انجام شد. شما به صفحه فعالسازی هدایت می‌شوید.";
            return RedirectToAction("Activate", "Accounts");
        }

        public IActionResult Activate()
        {
            if (CheckLogin())
            {
                string username = User.Identity.Name;
                string roleName = _user.GetUserRoleName(username);
                if (roleName == "کاربر")
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                else if (roleName == "فروشگاه")
                {
                    return RedirectToAction("Dashboard", "Panel");

                }
                else if (roleName == "مدیر")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Activate(ActivateViewModel viewModel)
        {
            // 1. بررسی اعتبارسنجی سمت سرور
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["ErrorTitle"] = "خطا در فعالسازی";
                TempData["ErrorMessage"] = "اطلاعات وارد شده نامعتبر است. " + string.Join(" ", errors);
                return RedirectToAction("Register", "Accounts");
            }

            // 2. فعال‌سازی کاربر
            if (_account.ActivateUser(NumberConvertor.ToEnglishNumber(viewModel.ActiveCode)))
            {
                TempData["SuccessTitle"] = "فعال‌سازی موفق";
                TempData["SuccessMessage"] = "حساب کاربری شما با موفقیت فعال شد. اکنون به صفحه ورود منتقل می‌شوید.";
                return RedirectToAction("Login", "Accounts");
            }
            else
            {
                TempData["ErrorTitle"] = "خطا در فعالسازی";
                TempData["ErrorMessage"] = "کد فعالسازی صحیح نمی‌باشد.";
                return RedirectToAction("Activate", "Accounts");
            }
        }

        public IActionResult Login()
        {
            if (CheckLogin())
            {
                string username = User.Identity.Name;
                string roleName = _user.GetUserRoleName(username);
                if (roleName == "کاربر")
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                else if (roleName == "فروشگاه")
                {
                    return RedirectToAction("Dashboard", "Panel");

                }
                else if (roleName == "مدیر")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string HashPassword = HashGenerators.MD5Encoding(viewModel.Password);
                string Mobile = NumberConvertor.ToEnglishNumber(viewModel.Mobile);
                User user = _account.LoginUser(Mobile, HashPassword);

                if (user != null)
                {
                    var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Mobile.ToString())
            };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(principal, properties);

                    if (user.Role.Name == "کاربر")
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Panel");
                    }
                }
                else
                {
                    TempData["ErrorTitle"] = "خطا در ورود";
                    TempData["ErrorMessage"] = "مشخصات کاربری صحیح نمی‌باشد.";
                    return RedirectToAction("Login", "Accounts");
                }
            }
            else
            {
                // اعتبارسنجی سمت سرور نامعتبر
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["ErrorTitle"] = "خطا در ورود";
                TempData["ErrorMessage"] = string.Join(" ", errors);
                return RedirectToAction("Login", "Accounts");
            }
        }


        public IActionResult Forget()
        {
            if (CheckLogin())
            {
                string username = User.Identity.Name;
                string roleName = _user.GetUserRoleName(username);
                if (roleName == "کاربر")
                {
                    return RedirectToAction("Dashboard", "Home");
                }
                else if (roleName == "فروشگاه")
                {
                    return RedirectToAction("Dashboard", "Panel");

                }
                else if (roleName == "مدیر")
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Forget(ForgetViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_account.ExistMobileNumber(NumberConvertor.ToEnglishNumber(viewModel.Mobile)))
                {
                    User user = new User();
                    user.ActiveCode = CodeGenerators.ActiveCode();
                    try
                    {
                        _messageSender.SMS(viewModel.Mobile,
                            "امکان تغییر گذرواژه با کد تأیید " +
                            _account.GetUserActiveCode(NumberConvertor.ToEnglishNumber(viewModel.Mobile)));

                        TempData["SuccessTitle"] = "کد ارسال شد";
                        TempData["SuccessMessage"] = "کد تأیید به شماره موبایل شما ارسال گردید.";
                    }
                    catch
                    {
                        TempData["ErrorTitle"] = "خطا در ارسال پیامک";
                        TempData["ErrorMessage"] = "مشکلی در ارسال پیامک پیش آمد، لطفاً دوباره تلاش کنید.";
                    }
                    return RedirectToAction(nameof(ResetPassword));
                }
                else
                {
                    TempData["ErrorTitle"] = "کاربر یافت نشد";
                    TempData["ErrorMessage"] = "کاربری با این شماره موبایل یافت نشد.";
                    return RedirectToAction(nameof(Forget));
                }
            }

            TempData["ErrorTitle"] = "خطا در اطلاعات";
            TempData["ErrorMessage"] = "لطفاً اطلاعات را به درستی وارد کنید.";
            return View(viewModel);
        }


        public IActionResult ResetPassword()
        {
            return View();
        }


        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_account.ResetPassword(NumberConvertor.ToEnglishNumber(viewModel.ActiveCode), viewModel.Password))
                {
                    TempData["SuccessTitle"] = "گذرواژه تغییر کرد";
                    TempData["SuccessMessage"] = "گذرواژه شما با موفقیت تغییر یافت. لطفاً دوباره وارد شوید.";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["ErrorTitle"] = "خطا در اعتبارسنجی";
                    TempData["ErrorMessage"] = "کد تأیید وارد شده صحیح نمی‌باشد.";
                    return RedirectToAction(nameof(ResetPassword));
                }
            }

            TempData["ErrorTitle"] = "خطا در ورود اطلاعات";
            TempData["ErrorMessage"] = "لطفاً اطلاعات را به‌درستی وارد کنید.";
            return View(viewModel);

        }

        public IActionResult Store()
        {
            ViewBag.MyMessage = false;
            return View();
        }

        [HttpPost]
        public IActionResult Store(StoreRegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_account.ExistsEmailAddress(viewModel.Mail))
                {
                    TempData["ErrorTitle"] = "خطا در ثبت فروشگاه";
                    TempData["ErrorMessage"] = "ایمیل وارد شده قبلاً برای ثبت فروشگاه استفاده شده است.";
                    return RedirectToAction(nameof(Store));
                }
                else
                {
                    int userID = 0;
                    string mobileCode = "";

                    if (_account.ExistMobileNumber(NumberConvertor.ToEnglishNumber(viewModel.Mobile)))
                    {
                        _account.UpdateUserRole(NumberConvertor.ToEnglishNumber(viewModel.Mobile));
                        userID = _account.GetUserID(NumberConvertor.ToEnglishNumber(viewModel.Mobile));
                        mobileCode = _account.GetUserActiveCode(NumberConvertor.ToEnglishNumber(viewModel.Mobile));
                    }
                    else
                    {
                        mobileCode = CodeGenerators.ActiveCode();

                        User user = new User()
                        {
                            ActiveCode = mobileCode,
                            Code = null,
                            FullName = null,
                            IsActive = false,
                            Mobile = NumberConvertor.ToEnglishNumber(viewModel.Mobile),
                            Password = HashGenerators.MD5Encoding(viewModel.Password),
                            Date = pc.GetYear(DateTime.Now).ToString("0000") + "/" +
                                   pc.GetMonth(DateTime.Now).ToString("00") + "/" +
                                   pc.GetDayOfMonth(DateTime.Now).ToString("00"),
                            RoleId = _account.GetStoreRole()
                        };
                        _account.AddUser(user);
                        userID = user.Id;
                    }

                    Store store = new Store()
                    {
                        UserId = userID,
                        Address = null,
                        Description = null,
                        Mail = viewModel.Mail,
                        logo = null,
                        MailActivate = false,
                        MobileActivate = false,
                        Tel = null,
                        Name = null,
                        MailActivateCode = CodeGenerators.ActiveCode()
                    };
                    _account.AddStore(store);

                    string messageBody = _render.RenderToStringAsync("_ActivateMail", store);
                    try
                    {
                        _messageSender.SMS(viewModel.Mobile, "درخواست ثبت فروشگاه انجام شد" + Environment.NewLine + "کد فعالسازی: " + mobileCode);
                        _messageSender.Email(store.Mail, "فعالسازی فروشگاه", messageBody);

                        TempData["SuccessTitle"] = "درخواست ثبت فروشگاه موفق";
                        TempData["SuccessMessage"] = "درخواست شما برای ثبت فروشگاه با موفقیت انجام شد. کد فعالسازی برای شماره موبایل و ایمیل شما ارسال گردید.";
                        return RedirectToAction(nameof(Store));
                    }
                    catch (Exception ex)
                    {
                        TempData["ErrorTitle"] = "خطا در ارسال پیام";
                        TempData["ErrorMessage"] = "در ارسال پیامک یا ایمیل خطایی رخ داد: " + ex.Message;
                        return RedirectToAction(nameof(Store));
                    }
                }
            }

            TempData["ErrorTitle"] = "خطا در اطلاعات ورودی";
            TempData["ErrorMessage"] = "لطفاً تمام فیلدها را به‌درستی تکمیل کنید.";
            return View(viewModel);
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

    }
}