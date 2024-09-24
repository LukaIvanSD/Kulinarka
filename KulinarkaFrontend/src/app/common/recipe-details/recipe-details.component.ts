import { CommonModule } from '@angular/common';
import { Component,OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RecipeService } from '../../Service/recipe.service';
import { RecipeDetails } from '../../model/recipeDetails';
import { Ingredient } from '../../model/recipeDetails';
import { PreparationStep } from '../../model/recipeDetails';
import { Tag } from '../../model/recipeDetails';
import { TagService } from '../../Service/tag.service';
@Component({
  selector: 'app-recipe-details',
  standalone: true,
  imports: [ReactiveFormsModule,FormsModule,CommonModule],
  templateUrl: './recipe-details.component.html',
  styleUrl: './recipe-details.component.css'
})
export class RecipeDetailsComponent implements OnInit {
  recipeBackup!: RecipeDetails;
  recipe: RecipeDetails = {
    recipe:{
    id: -1,
    userId: -1,
    name: '',
    description: '',
    duration: 0,
    numberOfPeople: 0,
    difficulty: '',
    chefsAdvice: '',
    pictureBase64: '',
    creationDate: new Date(),
    },
    owner: {
      id: -1,
      username: '',
      pictureBase64: '',
      dateOfCreation: new Date(),
      title: ''
    },
    ingredients: [],
    steps: [],
    tags: [],
    isPromoted: false
  };
  isEditRecipeModalOpen = false;
  isEditIngredientsModalOpen = false;
  isEditStepsModalOpen = false;
  isEditTagsModalOpen = false;
  isUserOwnerOfRecipe=false;
  hasUpdatedRecipe=false;
  availableTags: Tag[] = [];
  constructor(private recipeService: RecipeService,private route: ActivatedRoute,private tagService: TagService) {
  }

  ngOnInit(): void {
    const recipeId = Number(this.route.snapshot.paramMap.get('id'));
      this.recipeService.GetRecipeDetails(recipeId).subscribe({
    next: (recipe : RecipeDetails) => {
    this.recipe = recipe;
    this.recipeBackup = JSON.parse(JSON.stringify(recipe));
    console.log('Detalji recepta:', this.recipe);
    },
    error: (error) => {
    console.error('Došlo je do greške:', error);
    }});
    this.recipeService.IsUserOwnerOfRecipe(recipeId).subscribe({
      next: (isOwner : boolean) => {
      this.isUserOwnerOfRecipe = isOwner;
      console.log('Da li je korisnik vlasnik recepta:', this.isUserOwnerOfRecipe);
      },
      error: (error) => {
      console.error('Došlo je do greške:', error);
      }});
  }





  OpenEditRecipeInfoModal(): void {
    this.isEditRecipeModalOpen = true;
  }

  CancelEditRecipeInfo(): void {
    this.isEditRecipeModalOpen = false;
    this.recipe.recipe = JSON.parse(JSON.stringify(this.recipeBackup.recipe));
  }

  SaveRecipeInfoChanges(): void {
    this.hasUpdatedRecipe = true;
    this.isEditRecipeModalOpen = false;
  }



  OpenEditStepsModal(): void {
    this.isEditStepsModalOpen = true;
  }

  CancelEditStepsModal(): void {
    this.isEditStepsModalOpen = false;
    this.recipe.steps = JSON.parse(JSON.stringify(this.recipeBackup.steps));
  }

  SaveStepsChanges(): void {
    this.hasUpdatedRecipe = true;
    this.isEditStepsModalOpen = false;
  }
  AddStep():void
  {
    this.recipe.steps.push({description: '', sequenceNumber: this.recipe.steps.length + 1});
  }
  DeleteStep(step:PreparationStep): void
  {
    const index = this.recipe.steps.findIndex(s => s === step);
    if (index > -1) {
      this.recipe.steps.splice(index, 1);
      for (let i = index; i < this.recipe.steps.length; i++) {
        this.recipe.steps[i].sequenceNumber--;
      }
    }
  }

  LoadAvailableTags(): void {
    this.tagService.GetTags().subscribe({
      next: (tags: Tag[]) => {
        this.availableTags = tags.filter(t => !this.recipe.tags.some(rt => rt.name === t.name));
        console.log('Dostupni tagovi:', this.availableTags);
      },
      error: (error) => {
        console.error('Došlo je do greške:', error);
      }
    });
  }


  OpenEditTagsModal(): void {
    this.isEditTagsModalOpen = true;
    this.LoadAvailableTags();
  }

  CancelEditTagsModal(): void {
    this.isEditTagsModalOpen = false;
    this.recipe.tags = JSON.parse(JSON.stringify(this.recipeBackup.tags));
  }
  SaveTagsChanges(): void {
    this.hasUpdatedRecipe = true;
    this.isEditTagsModalOpen = false;
  }
  DeleteTag(tag:Tag): void
  {
    const index = this.recipe.tags.indexOf(tag);
    this.recipe.tags.splice(index, 1);
    this.availableTags.push(tag);
  }
  AddTagToRecipe(tag:Tag): void
  {
    this.recipe.tags.push(tag);
    const index = this.availableTags.indexOf(tag);
    this.availableTags.splice(index, 1);
  }



  OpenEditIngredientsModal(): void {
    this.isEditIngredientsModalOpen = true;
  }

  CancelEditIngredientsModal(): void {
    this.recipe.ingredients = JSON.parse(JSON.stringify(this.recipeBackup.ingredients));
    this.isEditIngredientsModalOpen = false;
  }

  SaveIngredientsChanges(): void {
    this.hasUpdatedRecipe = true;
    this.isEditIngredientsModalOpen = false;
  }
  DeleteIngredient(ingredient:Ingredient): void
  {
    const index = this.recipe.ingredients.indexOf(ingredient);
    this.recipe.ingredients.splice(index, 1);
  }
  AddIngredient():void
  {
    this.recipe.ingredients.push({name: '', measurementUnit: '', amount: 0});
  }


  UpdateRecipe(): void {
    console.log('Ažuriranje recepta:', this.recipe);
    this.recipeService.UpdateRecipe(this.recipe).subscribe({
      next: () => {
        this.recipeBackup = JSON.parse(JSON.stringify(this.recipe));
        this.hasUpdatedRecipe = false;
        console.log('Recept je uspešno ažuriran!');
      },
      error: (error:any) => {
        console.error('Došlo je do greške:', error);
      }});
    }

  CancelUpdateRecipe(): void {
    this.recipe = JSON.parse(JSON.stringify(this.recipeBackup));
    this.hasUpdatedRecipe = false;
  }
}
