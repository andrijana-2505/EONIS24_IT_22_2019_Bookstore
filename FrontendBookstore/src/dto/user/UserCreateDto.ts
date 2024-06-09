interface UserCreateDto {
  username: string;
  password: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  gender?: string;
}

export default UserCreateDto;
