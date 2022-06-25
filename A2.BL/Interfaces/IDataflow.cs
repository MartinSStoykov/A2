using System.Threading.Tasks;

namespace A2.BL.Interfaces
{
    public interface IDataflow
    {
        Task SendMotor(byte[] data);
    }
}