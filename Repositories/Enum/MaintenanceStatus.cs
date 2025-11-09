using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Enum
{
    public enum MaintenanceStatus
    {
        DaLenLich = 1,    // Đã lên lịch
        QuaHan = 2,       // Quá hạn
        DangThucHien = 3, // Đang thực hiện
        HoanThanh = 4     // Hoàn thành
    }
}
