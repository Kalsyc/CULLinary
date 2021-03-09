using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingController : MonoBehaviour
{
    public GameObject[] foodOnCounter; // Can edit later to make this dynamic as more food gets cooked
    public GameObject player;
    public Camera cam;

    public GameObject[] foodPrefabs;
    public int foodId;

    public bool holdingItem = false;
    public Transform foodLocation;
    private GameObject currCustomer = null; // to store current customer player is serving

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetMouseButton(0))
        {
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Pick up food from counter
                if (hit.collider != null && hit.collider.gameObject.tag == "FoodToServe")
                {
                    string selectedDish = hit.collider.gameObject.name;
                    if (selectedDish == ("plateDinner(Clone)") && !holdingItem)
                    {
                        Debug.Log("Picked up plate");
                        foodId = 0;
                        CollectFood();
                        Destroy(hit.collider.gameObject); // comment this out if want unlimited servings of the dish after cooking
                    }
                    else if (selectedDish == ("bowlBroth(Clone)") && !holdingItem)
                    {
                        Debug.Log("Picked up bowl");
                        foodId = 1;
                        CollectFood();
                        Destroy(hit.collider.gameObject); // comment this out if want unlimited servings of the dish after cooking
                    }
                }

                // Serve customer
                if (hit.collider != null && hit.collider.gameObject.tag == "Customer")
                {
                    // Serve the customer if carrying food
                    Debug.Log("Clicked Customer");
                    if (holdingItem)
                    {
                        Debug.Log("Serving Customer!");
                        currCustomer = hit.collider.gameObject;
                        ServeFood(currCustomer);
                    }
                }
            }
        }
    }

    // Shift food so player looks like carrying the food
    // Remove food from counter accordingly
    void CollectFood()
    {
        holdingItem = true;
   
        GameObject plateDish = Instantiate(foodPrefabs[foodId], foodLocation.position, Quaternion.identity);
        plateDish.transform.SetParent(foodLocation);

        //Destroy(foodOnCounter[foodId]);
    }

    // Shift food so player looks has served food to customer
    // Remove food from player's hands accordingly
    void ServeFood(GameObject customer)
    {
        Transform serveLocation = customer.transform.Find("Serve Food Location");

        GameObject plateDish = Instantiate(foodPrefabs[foodId], serveLocation.position, Quaternion.identity);
        plateDish.transform.SetParent(serveLocation);

        holdingItem = false; // reset bool value since not carrying anything anymore

        var foodLocation = player.transform.Find("FoodLocation"); // Destroy food the player was carrying
        GameObject holdingPos = foodLocation.gameObject;

        foreach (Transform child in holdingPos.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        currCustomer = null;
    }

    }
