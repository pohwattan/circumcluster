using UnityEngine;
public class OutOfBounds : MonoBehaviour
{
    private Rigidbody2D rb;
    float x,y;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        y = Camera.main.orthographicSize;
        x = Camera.main.aspect*y;
    }

    void Update()
    {
        if(transform.position.x > x && rb.velocity.x>0){
            transform.position = new Vector2(-x,transform.position.y);
        }
        else if(transform.position.x< -x && rb.velocity.x<0){
            transform.position = new Vector2(x,transform.position.y);
        }
        if(transform.position.y > y && rb.velocity.y>0){
            transform.position = new Vector2(transform.position.x,-y);
        }
        else if(transform.position.y< -y && rb.velocity.y<0){
            transform.position = new Vector2(transform.position.x,y);
        }
    }

    public float getDistance(Vector3 from, Vector3 to){
        Vector3 coords = getCoords(to);
        float magnitude = (x+y)*2;
        for(int _x = -1; _x<2; _x++){
            for(int _y = -1; _y<2; _y++){
                if(Vector3.Distance(from,coords + new Vector3(_x*x*2, _y*y*2,0)) < magnitude){
                    magnitude = Vector3.Distance(from,coords+ new Vector3(_x*x*2, _y*y*2,0));
                }
            }
        }
        return magnitude;
    }
    public Vector3 getDirection(Vector3 from,Vector3 to){
        Vector3 coords = getCoords(to);
        Vector3 result = Vector3.zero;
        float magnitude = (x+y)*2;
        for(int _x = -1; _x<2; _x++){
            for(int _y = -1; _y<2; _y++){
                if(Vector3.Distance(from,coords + new Vector3(_x*x*2, _y*y*2,0)) < magnitude){
                    magnitude = Vector3.Distance(from,coords+ new Vector3(_x*x*2, _y*y*2,0));
                    result = coords + new Vector3(_x*x*2, _y*y*2,0);
                }
            }
        }
        //Debug.DrawLine(from,result);
        return from-result;
    }

    public Vector3 getCoords(Vector3 vect){
        float _x = vect.x;
        float _y = vect.y;
        while(_x>x || _x<-x){
            _x = (_x>x)?_x-x*2:_x+x*2;
        }
        while(_y>y || _y<-y){
            _y = (_y>y)?_y-y*2:_y+y*2;
        }
        return new Vector2(_x, _y);
    }
}
