import { Component, OnInit } from '@angular/core';
import { RecipeService } from '../../Service/recipe.service';
import { UserRecipe } from '../../model/userRecipe';
import { CommonModule } from '@angular/common';
import { DeleteModalComponent } from '../delete-modal/delete-modal.component';
import { Recipe as GeneralRecipe } from '../../model/recipe';
import { FormsModule } from '@angular/forms';
import { Tag } from '../../model/tag';
import { filter } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-recipes',
  standalone: true,
  imports: [CommonModule,DeleteModalComponent,FormsModule],
  templateUrl: './user-recipes.component.html',
  styleUrl: './user-recipes.component.css'
})
export class UserRecipesComponent implements OnInit {
  recipes!:UserRecipe[];
  searchText: string = '';
  recipesToShow:UserRecipe[]=[];
  suggestions: string[]=[];
  showDeleteModal:boolean =false;
  selectedRecipe: UserRecipe | undefined;
  canPromote:boolean=true;
  mealTimes : string[]=[];
  preparationMethods : string[]= [];
  flavors :string[]=[];
  selectedMealTime:string []=[];
  selectedPreparationMethod:string []=[];
  selectedFlavor:string []=[];
constructor(private recipeService:RecipeService,private router:Router) {
}
ngOnInit(): void {
  this.recipeService.GetUserRecipes().subscribe({
    next:(data:UserRecipe[])=>{
      this.recipes=data;
      this.recipesToShow=this.recipes;
      this.SortRecipesBy('Promotion');
      this.recipes.forEach(recipe=>{
        recipe.tags.forEach(tag=>{
          if(tag.tagType==='MealTime' && !this.mealTimes.includes(tag.name))
            this.mealTimes.push(tag.name);
          else if(tag.tagType==='PreparationMethod' && !this.preparationMethods.includes(tag.name))
            this.preparationMethods.push(tag.name);
          else if(tag.tagType==='Flavor' && !this.flavors.includes(tag.name))
            this.flavors.push(tag.name);
        });
      });
      console.log(this.recipes);
    },
    error:(err)=>{
      console.log(err);
    }
  });
}
ShowDeleteModal(recipe : UserRecipe):void{
this.showDeleteModal=true;
this.selectedRecipe=recipe;
}
HandleCloseModal():void{
  this.showDeleteModal=false;
  this.selectedRecipe=undefined;
}
HandleDeleteRecipe(deletedRecipe:GeneralRecipe)
{
  this.recipes=this.recipes.filter(recipe=>recipe.recipe.id!==deletedRecipe.id);
  this.recipesToShow=this.recipes;
  this.HandleCloseModal();
}
ShowSearched(){ 
  this.suggestions=[];
  this.recipesToShow=this.recipes.filter(recipe=>recipe.recipe.name.toLowerCase().includes(this.searchText.toLowerCase()));
  if(this.searchText!=='')
  this.recipes.forEach(recipe=>{
    if(recipe.recipe.name.includes(this.searchText))
      this.suggestions.push(recipe.recipe.name);
  });
}
SelectSuggestion(selectedSuggestion : string){
  this.searchText=selectedSuggestion;
  this.ShowSearched();
  this.suggestions=[];  
}
PromoteRecipe(selectedRecipe:UserRecipe):void{
  this.recipeService.PromoteRecipe(selectedRecipe.recipe.id).subscribe({
    next:(data)=>{
      this.recipes.forEach(recipe=>{
        if(recipe.recipe.id===selectedRecipe.recipe.id)
          recipe.isPromoted=true;
      });
      this.recipesToShow=this.recipes;
      console.log(data);
    },
  error:(err)=>{
    console.log(err);
  }
  });
}
Reverse():void{
  this.recipesToShow=this.recipesToShow.reverse();
}
SortRecipes(event:Event):void{
  this.SortRecipesBy((event.target as HTMLSelectElement).value);
}
SortRecipesBy(criteria:string):void{
  switch(criteria){
    case 'Date':
      this.recipesToShow.sort((a,b)=>new Date(b.recipe.creationDate).getTime()-new Date(a.recipe.creationDate).getTime());
      break;
    case 'Promotion':
      this.recipesToShow.sort((a,b)=>(b.isPromoted?1:0)-(a.isPromoted?1:0));
      break;
    case 'Alphabet':
      this.recipesToShow.sort((a,b)=>a.recipe.name.localeCompare(b.recipe.name));
      break;
      case 'Duration':
        this.recipesToShow.sort((a,b)=>a.recipe.duration-b.recipe.duration);
        break;
      case 'People':
        this.recipesToShow.sort((a,b)=>a.recipe.numberOfPeople-b.recipe.numberOfPeople);
        break;
      case 'Difficulty':
        this.recipesToShow.sort((a,b)=>this.GetDifficultyAsNumber(a.recipe.difficulty)-this.GetDifficultyAsNumber(b.recipe.difficulty));
        break;
  }
}
GetDifficultyAsNumber(difficulty:string):number{
  switch(difficulty){
    case 'Easy':
      return 1;
    case 'Medium':
      return 2;
    case 'Hard':
      return 3;
      default:
        return 0;
}
}
OnCheckboxChange(tagType:string,tagName: string, event: Event): void {
  const checkbox = (event.target as HTMLInputElement).checked;

  if (checkbox) {
    switch(tagType){
      case 'MealTime':
        this.selectedMealTime.push(tagName);
        break;
      case 'PreparationMethod':
        this.selectedPreparationMethod.push(tagName);
        break;
      case 'Flavor':
        this.selectedFlavor.push(tagName);
        break;
    }
  } else {
    switch(tagType){
      case 'MealTime':
        this.selectedMealTime=this.selectedMealTime.filter(tag=>tag!==tagName);
        break;
      case 'PreparationMethod':
        this.selectedPreparationMethod=this.selectedPreparationMethod.filter(tag=>tag!==tagName);
        break;
      case 'Flavor':
        this.selectedFlavor=this.selectedFlavor.filter(tag=>tag!==tagName);
        break;
    }
  }
  if(this.selectedFlavor.length===0 && this.selectedMealTime.length===0 && this.selectedPreparationMethod.length===0)
    this.recipesToShow = this.recipes;
  else
    this.FilterRecipes();
}
FilterRecipes():void{
  this.recipesToShow = this.recipes.filter(recipe => {
    let hasFlavor=false;
    if(this.selectedFlavor.length!==0)
    hasFlavor = recipe.tags.some(tag => 
      tag.tagType === 'Flavor' && this.selectedFlavor.includes(tag.name)
    );
    let hasMealTime=false;
    if(this.selectedMealTime.length!==0)
     hasMealTime = recipe.tags.some(tag => 
      tag.tagType === 'MealTime' && this.selectedMealTime.includes(tag.name)
    );
    let hasPreparationMethod=false;
    if(this.selectedPreparationMethod.length!==0)
     hasPreparationMethod = recipe.tags.some(tag => 
      tag.tagType === 'PreparationMethod' && this.selectedPreparationMethod.includes(tag.name)
    );
    return hasFlavor || hasMealTime || hasPreparationMethod;
  });
}
ShowRecipeDetails(recipeId: number):void{
    this.router.navigate(['/recipeDetails', recipeId]);
}
}
