using A2.Models;
using System.Threading.Tasks;

namespace A2.BL.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProduceMotor(Motor motor);
    }
}