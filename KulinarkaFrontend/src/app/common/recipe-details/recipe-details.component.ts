import { CommonModule } from '@angular/common';
import { Component,OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RecipeService } from '../../Service/recipe.service';
import { RecipeDetails } from '../../model/recipeDetails';
import { Ingredient } from '../../model/recipeDetails';
import { PreparationStep } from '../../model/recipeDetails';
import { Tag } from '../../model/recipeDetails';
import { TagService } from '../../Service/tag.service';
import { CommentService } from '../../Service/comment.service';
import { Comment } from '../../model/recipeDetails';
import { PreparedRecipeImage } from '../../model/recipeDetails';
import { CommentWithImage } from '../../model/recipeDetails';
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
    comments: [],
    preparedRecipeImages: [],
    isPromoted: false
  };
  newComment ={
  text: '',
  header: '',
  recipeId: -1
  };
  isEditRecipeModalOpen = false;
  isEditIngredientsModalOpen = false;
  isEditStepsModalOpen = false;
  isEditTagsModalOpen = false;
  isUserOwnerOfRecipe=false;
  hasUpdatedRecipe=false;
  availableTags: Tag[] = [];
  commentPageNumber = 1;
  commentPageSize = 5;
  picturePageNumber = 1;
  picturePageSize = 5;
  hasMoreComments = true;
  hasMorePictures = true;
  hasUploadedPreparedRecipeImage = false;
  showExtendedCommentForm = false;
  showCommentForm = false;
  selectedImage :File | null = null;
  isUploadingImage = false;
  constructor(private recipeService: RecipeService,private route: ActivatedRoute,private tagService: TagService,private commentService: CommentService,private router:Router) {
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
      this.recipeService.HasUserUploadedImage(recipeId).subscribe({
        next: (hasUploaded : boolean) => {
        this.hasUploadedPreparedRecipeImage = hasUploaded;
        console.log('Da li je korisnik postavio sliku:', this.hasUploadedPreparedRecipeImage);
        },
        error: (error :any) => {
        console.error('Došlo je do greške:', error);
        }});
  }



  ShowAddCommentForm(): void {
    if(this.hasUploadedPreparedRecipeImage)
      this.showCommentForm = true;
    else
      this.showExtendedCommentForm = true;
  }
  CancelAddCommentForm(): void {
    this.showCommentForm = false;
    this.showExtendedCommentForm = false;
    this.newComment.text = '';
    this.newComment.header = '';
    this.selectedImage = null;
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
  MoreComments(): void {
    this.commentPageNumber++;
    this.commentService.GetComments(this.recipe.recipe.id, this.commentPageNumber, this.commentPageSize).subscribe({
      next: (comments: Comment[]) => {
        this.recipe.comments.push(...comments);
        if(comments.length < this.commentPageSize)
          this.hasMoreComments = false;
        console.log('Komentari:', this.recipe.comments);
      },
      error: (error) => {
        console.error('Došlo je do greške:', error);
      }
    });
  }
  MorePictures(): void {
    this.picturePageNumber++;
    this.recipeService.GetPreparedRecipeImages(this.recipe.recipe.id, this.picturePageNumber, this.picturePageSize).subscribe({
      next: (images: PreparedRecipeImage[]) => {
        this.recipe.preparedRecipeImages.push(...images);
        if(images.length < this.picturePageSize)
          this.hasMorePictures = false;
        console.log('Slike:', this.recipe.preparedRecipeImages);
      },
      error: (error:any) => {
        console.error('Došlo je do greške:', error);
      }
    });
  }

  ShowProfile(userId : number): void {
    //this.router.navigate(['/profile', userId]);
    console.log('Prikaz profila korisnika sa ID:', userId);
    }
    UploadImage(event:Event): void {
      console.log('Dodavanje slike...');
      const file = (event.target as HTMLInputElement).files![0];
      if(file)
      {
        this.selectedImage = file;
        this.isUploadingImage = true;
      }
    }
    PostImage(): void {
      let form =new FormData();
      form.append('image',this.selectedImage!);
      form.append('recipeId',this.recipe.recipe.id.toString());
      this.recipeService.UploadPreparedRecipeImage(form).subscribe({
        next: (image: PreparedRecipeImage) => {
          this.recipe.preparedRecipeImages.push(image);
          console.log('Slika je uspešno dodata!');
          this.hasUploadedPreparedRecipeImage = true;
          this.isUploadingImage = false;
        },
        error: (error:any) => {
          console.error('Došlo je do greške:', error);
        }
      });
    }
    CancelUploadImage(): void {
      this.selectedImage = null;
      this.isUploadingImage = false;
    }
    AddComment(): void {
      this.newComment.recipeId = this.recipe.recipe.id;
      this.commentService.AddComment(this.newComment).subscribe({
        next: (comment: Comment) => {
          this.recipe.comments.push(comment);
          console.log('Komentar je uspešno dodat!');
        },
        error: (error:any) => {
          console.error('Došlo je do greške:', error);
        }
      });
    }
    AddExtendedComment(): void {
      this.newComment.recipeId = this.recipe.recipe.id;
      let form = new FormData();
      form.append('image', this.selectedImage!);
      form.append('recipeId', this.newComment.recipeId.toString());
      form.append('header', this.newComment.header);
      form.append('text', this.newComment.text);
      this.commentService.AddExtendedComment(form).subscribe({
        next: (comment: CommentWithImage) => {
          this.recipe.comments.push(comment.comment);
          this.recipe.preparedRecipeImages.push(comment.preparedRecipeImage);
          console.log(comment);
          console.log('Komentar je uspešno dodat!');
        },
        error: (error:any) => {
          console.error('Došlo je do greške:', error);
        }
      });
    }
}
