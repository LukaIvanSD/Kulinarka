import { Component,Input,Output,EventEmitter } from '@angular/core';
import { RecipeService } from '../../Service/recipe.service';
import { Recipe } from '../../model/userRecipe';
import { Recipe as GeneralRecipe} from '../../model/recipe';


@Component({
  selector: 'app-delete-modal',
  standalone: true,
  imports: [],
  templateUrl: './delete-modal.component.html',
  styleUrl: './delete-modal.component.css'
})
export class DeleteModalComponent {
  @Input() recipe: Recipe | undefined;
  @Output() closeModal = new EventEmitter<void>();
  @Output() deleteRecipe = new EventEmitter<GeneralRecipe>();
  constructor(private recipeService:RecipeService){}
  DeleteRecipe():void{
    if (this.recipe?.id !== undefined)
    this.recipeService.DeleteRecipe(this.recipe.id).subscribe({
      next:(data:GeneralRecipe)=>{
        this.deleteRecipe.emit(data);
      },
      error:(err:string)=>
      {
        console.log(err);
      }
    })
    else
    {
    console.log('Recipe ID is undefined');
    }
  }
  Cancel():void{
    this.closeModal.emit();
  }
}
