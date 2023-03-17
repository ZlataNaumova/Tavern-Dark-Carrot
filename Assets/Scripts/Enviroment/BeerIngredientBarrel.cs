using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeerIngredientBarrel :PlayerInteractable
{
    [SerializeField] private int beerIngredientType;
    [SerializeField] private Sprite redBeerIngredientSprite;
    [SerializeField] private Sprite greenBeerIngredientSprite;
    [SerializeField] private Image currentBeerIngredientImage;

    private void Start() => currentBeerIngredientImage.sprite = beerIngredientType == 1 ? greenBeerIngredientSprite : redBeerIngredientSprite;

    public override void PlayerInteraction()
    {
        if(!player.isHoldingBeerKeg && !player.isHoldingCleaningMaterials && !player.isHoldingGlassOfBeer)
        {
            player.TakeBeerIngredient(beerIngredientType);
        }
        
    }

   
}
