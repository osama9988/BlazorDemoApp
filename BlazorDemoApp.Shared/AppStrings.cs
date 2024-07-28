using BlazorDemoApp.Shared.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDemoApp.Shared
{
    public class MyJsonRsult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Message2 { get; set; }
        public string? Url { get; set; }
        public bool? Deleted { get; set; }=null;
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public string DataType { get; set; }    
        public List<ApiError> Errors { get; set; }

        public static async Task<ApiResponse<T>> HandleExceptionAsync(Func<Task<ApiResponse<T>>> action)
        {
            try
            {
                var result = await action();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = e.Message 
                };
            }
        }
    }

    public class ApiError
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }
   
    public static class App_Strings
    {


        public static readonly List<Select2Model> qs = new List<Select2Model>() {

            new Select2Model(){ Id="", Text=""},
            new Select2Model(){ Id="1", Text="ما هو عنوان منزل والديك"},
            new Select2Model(){ Id="2", Text="ما هو رقم منزل والديك"},
            new Select2Model(){ Id="3", Text="ما هو لونك المفضل"},
            new Select2Model(){ Id="4", Text="ما هو فريقك المفضل"},
            new Select2Model(){ Id="5", Text="من هو كاتبك المقضل"},
            new Select2Model(){ Id="6", Text="ما هو المكان الذي تريد السفر اليه"},
            new Select2Model(){ Id="7", Text="ما هو اسم مدرستك الثانوية"},
            new Select2Model(){ Id="8", Text="ما هو اسم حيوانك الأليف"},
            new Select2Model(){ Id="9", Text="ما هي المقولة المفضلة لك"},
            new Select2Model(){ Id="10", Text="ما هو اول رقم موبايل لك"},
        };

        public static readonly List<Select2Model> List_isActive = new List<Select2Model>()
        {
                new Select2Model() { Id = "", Text = ""},
                new Select2Model() { Id = "0", Text = "غير نشيط"},
                new Select2Model() { Id = "1", Text = " نشيط"}
        };

        public enum TempDataKeys
        {
            note_ok,
            note_error,
            controllerName,
            Cur_Lang,
            cur_id,
            list1

        }
        public enum cur_lang
        {
            ar,
            en
        };

        public enum arab_lang_type
        {
            male,
            female
        };
        public enum op_type
        {
            ok,
            error
        }

        public enum DeleteResult
        {
            can_delete,
            cannot_delete,
            deleted,
            failed
        }

        public static string GetFirstNOrSpaces(string input, int n)
        {

            if (string.IsNullOrEmpty(input))
            {
                // If the input string is null or empty, return N spaces
                return new string(' ', n);
            }
            else if (input.Length >= n)
            {
                // If the input string is equal to or longer than N characters, return it as is
                return input.Substring(0, n);
            }
            else
            {
                // Use StringBuilder to pad the input string with spaces on the right to make it N characters long
                StringBuilder sb = new StringBuilder(input);
                sb.Append(' ', n - input.Length);
                return sb.ToString();
            }

        }

        public enum OrderDirection
        {
            Ascending,
            Descending
        }

        public static string App_name = "BlazorDemoApp";
        public static string App_name0 = "SchoolERP_V1";
        //
        public static string aj_call = "aj_call";
        //
        public static string No_Data = "لا توجد أي بيانات لعرضها";
        public static string Btn_Add_New = "إضافة عنصر جديد";
        public static string Btn_Search = "بحث";
        public static string Btn_Edit = "تعديل";
        public static string Btn_Delete = "حذف";
        public static string Btn_Status_change = "تغيير الحالة";
        public static string Btn_Print = "طباعة";
        public static string Btn_View = "عرض";
        public static string Btn_Save = "حفظ";
        public static string Btn_copy = "نسخ";
        public static string Btn_Close = "إغلاق";
        public static string Btn_Clear = "مسح";
        public static string Btn_Clear_inputs = "مسح كل المدخلات";
        public static string Btn_Cancel = "إلغاء";
        public static string Btn_Back_to_List = "العودة";
        public static string Btn_view_all = "عرض الكل";

        //
        public static string load_data_yes = "تم تحميل البيانات";
        public static string load_data_error = "حدث خطأ في تحميل البيانات";
        //
        public static string setPropError = "حدث خطأ في تحميل خطائص العنصر";
        //
        public static string str_delete_yes = "تم الحذف بنجاح";
        public static string str_delete_no = "حدث خطأ في عملية الحذف";
        public static string str_delete_edit_cannot = "لا يمكن حذف أو تعديل البيانات المطلوبة لأنها مرتبطة ببيانات أخرى";
        public static string str_save_yes = "تم حفظ البيانات بنجاح";
        public static string str_save_no = "حدث خطأ في عملية الحفظ";
        public static string str_save_not_yet = "لم يتم حفظ البيانات في قاعدة البيانات";
        public static string str_edit_yes = "تم تعديل البيانات بنجاح";
        public static string str_edit_no = "حدث خطأ في تعديل البيانات ";
        public static string Str_status_change_yes = "تم تعديل الحالة بنجاح";
        public static string Str_status_change_no = "حدث خطأ في تعديل الحالة";
        public static string Str_Serch_criteria = "معاملات البحث";
        public static string Str_Serch_filters = "معاملات تصفية نتائج البحث";
        public static string Str_error_cantbeEmpty = "لا يمكن ترك هذا الحقل فارغا";
        public static string Str_error_SelectOne = "يجب أن تختار عنصر واحد على الأقل";
        public static string Str_SelectOne = "اختار عنصر من هنا";
        public static string Str_ShowMore = "عرض المزيد";
        //
        public static string Btn_pinned_change = "تغيير حالة التثبيت ";
        public static string Btn_isShown_change = "تغيير حالة العرض ";

        public static string check_inputs = "تأكد من ادخال كل القيم المطلوبة";
        public static string pass_error = "لا يمكن اتمام العملية .. قمت بادخال كلمة سر حالية خاطئة";
        public static string ErrorSaveThisData = "حدث خطأ ما .. لا يمكن حفظ البيانات حاليا";
        public static string UserDataLoadError = "عفوا .. حدث خطأ في تحميل بيانات المستخدم";
        public static string NoPermissions = "عفوا .. لا تمتلك الصلاحيات الكافية لاتمام هذا الإجراء";
        public static string Edit_MyAppUserPassQs = "تم مسح كلمة السر الحالية و إعادتها للكلمة الافتراضية و كذلك اسئلة استرجاع البيانات";
        public static string Edit_MyAppUserPass = "تم مسح كلمة السر الحالية و إعادتها للكلمة الافتراضية";
        //

        public static string user_InActive = "تم ايقاف هذا المستخدم";
        public static string user_NotFound = "لا يوجد مستخدم مطابق للبيانات المدخلة";
        public static string user_MustChangePass = "يجب تغيير كلمة السر";
        //
    }
    public enum UserActions
    {
        [Description("جديد")]
        Insert,
        [Description("بحث")]
        search,
        [Description("تعديل")]
        Update,
        [Description("حذف")]
        Delete,
        [Description("طباعة")]
        print

    }


    public enum AppServices
    {
        [Description("إدارة مستخدمين الموقع")]
        UsersEmps,
        [Description("إدارة العنوان")]
        Address,
    }
}
