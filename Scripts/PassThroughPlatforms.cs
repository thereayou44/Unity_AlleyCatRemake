using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatforms : MonoBehaviour
{
    private Collider2D _colider;

    private bool _isPlayerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        _colider = GetComponent<Collider2D>();
    }

    private void SetPlayerOn(Collision2D colision, bool value)
    {
        var player = colision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            _isPlayerOn = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetPlayerOn(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetPlayerOn(collision, true);
    }

    // Update is called once per frame
    void Update()
    {
       if (_isPlayerOn && Input.GetAxisRaw("Vertical") < 0)
        {
            _colider.enabled = false;
            StartCoroutine(Waiter());
        }
  
    }

    private IEnumerator Waiter()
    {
        yield return new WaitForSeconds(0.3f);
        _colider.enabled = true;
    }
}
