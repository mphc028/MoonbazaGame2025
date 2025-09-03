using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{
    public Splatter splatter;

    private LineRenderer lineRenderer;

	private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update ()
    {
        AimAndFire();
    }

    private void AimAndFire()
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        lineRenderer.SetPosition(1, mouseWorldPosition);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, mouseWorldPosition);

            if (raycastHit)
            {
                Instantiate(splatter, raycastHit.point, Quaternion.identity);
            }
        }
    }
}
