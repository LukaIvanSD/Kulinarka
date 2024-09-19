import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormArray, Validators, FormControl } from '@angular/forms'; // Added FormControl
import { Tag } from '../../model/tag';
import { TagService } from '../../Service/tag.service';
import { RecipeService } from '../../Service/recipe.service';

@Component({
  selector: 'app-add-recipe',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './add-recipe.component.html',
  styleUrls: ['./add-recipe.component.css'] // corrected from styleUrl to styleUrls
})
export class AddRecipeComponent implements OnInit {
  recipeForm!: FormGroup;

  selectedImage: File | null = null;
  selectedImageContentType: string | null = null;
  selectedVideo: File | null = null;
  selectedVideoContentType: string | null = null;

  mealTimeTag: Tag[] = [];
  preparationMethodTag: Tag[] = [];
  flavorTag: Tag[] = [];

  selectedTags: Set<Tag> = new Set();

  constructor(private fb: FormBuilder, private tagService: TagService, private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.tagService.GetTags().subscribe({
      next: tags => {
        tags.forEach(tag => {
          if (tag.tagType === 'MealTime') {
            this.mealTimeTag.push(tag);
          } else if (tag.tagType === 'PreparationMethod') {
            this.preparationMethodTag.push(tag);
          } else if (tag.tagType === 'Flavor') {
            this.flavorTag.push(tag);
          }
        });
      },
      error: error => {
        console.error(error);
      }
    });

    this.recipeForm = this.fb.group({
      name: this.fb.control('', Validators.required),
      description: this.fb.control(''),
      duration: this.fb.control('', Validators.required),
      difficulty: this.fb.control('Easy'),
      numberOfPeople: this.fb.control(1),
      chefsAdvice: this.fb.control(''),
      ingredients: this.fb.array([]),
      steps: this.fb.array([]),
      tags: this.fb.control([]),
      image: this.fb.control(null),
      video: this.fb.control(null),
      contentType: this.fb.control(''),
    });
  }

  onTagChange(tag: Tag, event: Event): void {
    const isChecked = (event.target as HTMLInputElement).checked;
    if (isChecked) {
      this.selectedTags.add(tag);
    } else {
      this.selectedTags.delete(tag);
    }
  }


  CreateRecipe() {
    const formData = new FormData();
    const durationInMinutes = this.recipeForm.get('duration')?.value;
    const formattedDuration = this.formatDuration(durationInMinutes);

    let Recipe = {
      name: this.recipeForm.get('name')?.value,
      description: this.recipeForm.get('description')?.value,
      duration: formattedDuration,
      difficulty: this.recipeForm.get('difficulty')?.value,
      numberOfPeople: this.recipeForm.get('numberOfPeople')?.value,
      chefsAdvice: this.recipeForm.get('chefsAdvice')?.value,
    };

    formData.append('Recipe', JSON.stringify(Recipe));
    formData.append('Ingredients', JSON.stringify(this.recipeForm.get('ingredients')?.value));
    formData.append('PreparationSteps', JSON.stringify(this.recipeForm.get('steps')?.value));
    formData.append('Tags', JSON.stringify(Array.from(this.selectedTags)));
    
    if (this.selectedImage) {
      formData.append('Picture', this.selectedImage);
    }
    
    if (this.selectedVideo && this.selectedVideoContentType) {
      formData.append('video', this.selectedVideo);
      formData.append('contentType', this.selectedVideoContentType);
    } else {
      formData.append('video', '');
      formData.append('contentType', '');
    }

    this.recipeService.AddRecipe(formData).subscribe({
      next: data => {
        console.log(data);
      },
      error: error => {
        console.error(error);
      }
    });
  }

  formatDuration(seconds: number): string {
    const hours = Math.floor(seconds / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);
    const secs = seconds % 60;

    const paddedHours = String(hours).padStart(2, '0');
    const paddedMinutes = String(minutes).padStart(2, '0');
    const paddedSeconds = String(secs).padStart(2, '0');

    return `${paddedHours}:${paddedMinutes}:${paddedSeconds}`;
  }
  
get tags():FormArray{
  return this.recipeForm.get('tags') as FormArray;
}
  get steps():FormArray{
    return this.recipeForm.get('steps') as FormArray;
  }
  get ingredients(): FormArray {
    return this.recipeForm.get('ingredients') as FormArray;
  }
  get mealTypeTags() {
    return this.recipeForm.get('mealTypeTags') as FormArray;
}

get preparationMethodTags() {
    return this.recipeForm.get('preparationMethodTags') as FormArray;
}

get flavorTags() {
    return this.recipeForm.get('flavorTags') as FormArray;
}

  UploadMedia(event: Event, mediaType: string) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      if (mediaType === 'image') {
        this.selectedImage = fileInput.files[0];
        this.selectedImageContentType = this.selectedImage.type;
      } else if (mediaType === 'video') {
        this.selectedVideo = fileInput.files[0];
        this.selectedVideoContentType = this.selectedVideo.type;
      }
    }
  }
  AddIngredient(): void {
    const ingredientForm = this.fb.group({
      name: ['', Validators.required],
      amount: ['', Validators.required],
      measurementUnit: ['', Validators.required]
    });
    this.ingredients.push(ingredientForm);
  }
AddStep():void{
  const stepForm=this.fb.group({
    sequenceNumber:[this.steps.length+1],
    description:['',Validators.required]
  });
  this.steps.push(stepForm);
}
  removeStep(index:number):void{
    this.steps.removeAt(index);
  }
  removeIngredient(index: number): void {
    this.ingredients.removeAt(index);
  }
}
