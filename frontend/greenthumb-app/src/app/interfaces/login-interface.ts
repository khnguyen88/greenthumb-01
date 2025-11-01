export interface UserInterface {
  email: string;
  displayName: string;
  uid: string;
}

export interface UserRegisterInterface {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface UserLoginInterface {
  email: string;
  password: string;
}
