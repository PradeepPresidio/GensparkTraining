import { Component, computed, effect, signal } from '@angular/core';
import { RecipeModel } from '../models/recipe';
import { RecipeService } from '../services/recipe.service';
import { Recipe } from '../recipe/recipe';
@Component({
  selector: 'app-recipe-list',
  imports: [Recipe],
  templateUrl: './recipe-list.html',
  styleUrl: './recipe-list.css'
})
export class RecipeList {
  recipes = signal<RecipeModel[]>([]);
  constructor(private recipeService:RecipeService){

  }
  ngOnInit():void{
    this.recipeService.getAllRecipes().subscribe({
      next:(data:any)=>{
        this.recipes.set(data.recipes);
      },
      error:(err)=>{},
      complete:()=>{}
    })
  }
}
