using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float minVelocity = 0.1f; // Минимальная скорость для поворота

    private Vector3 _lastVelocity;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Получаем текущую скорость
        Vector3 velocity = rb.linearVelocity;
        
        // Игнорируем вертикальное движение (если нужно)
        velocity.y = 0;
        
        // Проверяем, что объект движется
        if (velocity.magnitude > minVelocity)
        {
            // Вычисляем целевое направление
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            
            // Плавно поворачиваем
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.fixedDeltaTime
            );
            
            _lastVelocity = velocity;
        }
    }
}
