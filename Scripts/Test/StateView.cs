using TMPro;
using UnityEngine;

public class StateView : MonoBehaviour
{
    [SerializeField] private FSMContext context;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text text2;

    private IMovementController mMovementController;

    private void Start()
    {
        mMovementController = context as IMovementController;
    }
    private void Update()
    {
        text.text = $"Type of current state: {context.CurrentState.GetType()}";
        text2.text = $"Velocity of context: {mMovementController.Velocity}";
    }
}
