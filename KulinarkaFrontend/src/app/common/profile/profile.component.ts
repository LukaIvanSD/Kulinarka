import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Profile } from '../../model/profile';
import { ProfileService } from '../../Service/profile.service';
import { FormsModule } from '@angular/forms';
import { UserAchievement } from '../../model/userAchievement';
import { UserAchievementService } from '../../Service/user-achievement.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit{
  showAchievements = false;
  userAchievements!: UserAchievement[];
  profile: Profile = {
    userInfo: {
      firstName: '',
      lastName: '',
      gender: '',
      birthday: new Date(),
      location: '',
      email: '',
      username: '',
      bio: '',
      dateOfCreation: new Date(),
      pictureBase64: ''
    },
    userStatistics: {
      numberOfLikes: 0,
      numberOfFollowers: 0,
      averageRating: 0,
      numberOfRecipes: 0,
      numberOfFavorites: 0
    },
    title: {
      name: '',
      aquiredDate: new Date(),
      numberOfCompletedAchievements: 0,
      nextTitleName: '',
      currentTitleRequirement: 0,
      nextTitleRequirement: 0
    },
    currentPromotionReward: {
      postsToPromote: 0,
      durationInDays: 0,
      intervalInDays: 0
    },
    nextPromotionReward: {
      postsToPromote: 0,
      durationInDays: 0,
      intervalInDays: 0
    }
  };
  profilePicture!: string;
  constructor(private profileService: ProfileService,private userAchievementService:UserAchievementService) {}

  ngOnInit(){
    this.profileService.GetProfile().subscribe({
      next: (data: Profile) => {
        this.profile = data;
        this.profilePicture = 'data:image/jpeg;base64,' + this.profile.userInfo.pictureBase64;

      },
      error: (err) => {
        console.error('Error getting profile:', err);
      }
    });
  }
  ShowAchievements(){
    this.showAchievements = !this.showAchievements;
    if (!this.userAchievements) {  // Provera da li su podaci već učitani
      this.userAchievementService.GetUserAchievements().subscribe({
        next: (data: UserAchievement[]) => {
          console.log(data);
          this.userAchievements = data;
        },
        error: (err) => {
          console.error('Error getting user achievements:', err);
        }
      });
    }
  }
}
