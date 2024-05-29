using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLNH.GR.Desktop.BO
{
    public class BaseEntity
    {
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? DeletedBy { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public int? EntityMode { get; set; } = 1;//0 = add, 1 = edit ... 



        /// <summary>
        /// hàm set giá trị cho property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void SetValue(string propertyName, object value)
        {
            this.GetType().GetProperty(propertyName)?.SetValue(this, value);
        }

        /// <summary>
        /// Hàm lấy giá trị theo tên thuộc tính
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object? GetValue(string propertyName)
        {
            return this.GetType().GetProperty(propertyName)?.GetValue(this, null);
        }
    }
}
