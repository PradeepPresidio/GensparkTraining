import { Component } from '@angular/core';
import { Products } from './products/products';
import { RecipeList } from './recipe-list/recipe-list';
@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [ Products,RecipeList]
})
export class App {
  protected title = 'myApp';
}