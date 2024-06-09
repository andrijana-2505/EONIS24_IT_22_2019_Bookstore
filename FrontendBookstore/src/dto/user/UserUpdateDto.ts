export interface UserUpdateDto {
  usersId: number;
  userRole: string; // or an enum if you have defined one
  username: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  genre?: string; // or char if you have defined one
}
