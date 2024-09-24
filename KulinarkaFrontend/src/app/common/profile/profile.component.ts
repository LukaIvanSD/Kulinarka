import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Profile, UserInfo } from '../../model/profile';
import { ProfileService } from '../../Service/profile.service';
import { FormsModule } from '@angular/forms';
import { UserAchievement } from '../../model/userAchievement';
import { UserAchievementService } from '../../Service/user-achievement.service';
import { User } from '../../model/user';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit{
  showAchievements = false;
  userInfo: UserInfo ={} as UserInfo;
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
  isEditing=false;
  isChangingPassword=false;
  profilePicture!: string;
  oldPassword!: string;
  newPassword!: string;
  confirmPassword!: string;
  isChangingPicture=false;
  selectedPicture!: FormData;
  constructor(private profileService: ProfileService,private userAchievementService:UserAchievementService) {}

  ngOnInit(){
    this.profileService.GetProfile().subscribe({
      next: (data: Profile) => {
        this.profile = data;
        this.userInfo = JSON.parse(JSON.stringify(data.userInfo));
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
  ChangePicture(){
    this.isChangingPicture = !this.isChangingPicture;
  }
  ChangeUserInfo(){
    this.isEditing = !this.isEditing;
  }
  CancelEditInfo(){
    this.profile.userInfo = JSON.parse(JSON.stringify(this.userInfo));
    this.isEditing = !this.isEditing;
  }
  SaveInfoChanges(){
    this.isEditing = !this.isEditing;
    this.profileService.UpdateUserInfo(this.profile.userInfo).subscribe({
      next: (data: User) => {
        this.userInfo = JSON.parse(JSON.stringify(this.profile.userInfo));
      },
      error: (err) => {
        console.error('Error updating user info:', err);
      }
    });
  }
  ChangePassword(){
    this.isChangingPassword = !this.isChangingPassword;
  }
  SavePasswordChanges(){
    this.isChangingPassword = !this.isChangingPassword;
    this.profileService.ChangePassword(this.oldPassword,this.newPassword).subscribe({
      next: (data: User) => {
        console.log('Password changed');
      },
      error: (err) => {
        console.error('Error changing password:', err);
      }
    });
  }
  CancelChangePassword(){
    this.isChangingPassword = !this.isChangingPassword;
  }
  UploadPicture(event:Event)
  {
    this.selectedPicture=new FormData();
    const files=(event.target as HTMLInputElement).files;
    if(files && files.length>0){
      this.selectedPicture.append('picture',files[0]);
  }
}
SavePictureChanges(){
  this.profileService.ChangePicture(this.selectedPicture).subscribe({
    next: (data: string) => {
      this.profile.userInfo.pictureBase64=data;
      this.profilePicture='data:image/jpeg;base64,'+data;
    },
    error: (err) => {
      console.error('Error uploading picture:', err);
}});
}
CancelChangePicture(){
  this.isChangingPicture = !this.isChangingPicture;
}
}
