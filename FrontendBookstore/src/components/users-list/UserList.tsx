import { useState, useEffect } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import useAuth from "../../hooks/useAuth";
import './UserList.css';
import { UserReadDto } from "../../dto/user/UserReadDto";
 

function ListUsers() {
  const [users, setUsers] = useState<UserReadDto[]>([]);
  const { userToken } = useAuth();

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const headers = { Authorization: `Bearer ${userToken?.token}` };
        const response = await axios.get("http://localhost:5137/api/User", { headers });
        setUsers(response.data);
      } catch (error) {
        console.error("Error fetching users:", error);
      }
    };

    fetchUsers();
  }, [userToken]);

  const deleteUser = (userId: number) => {
    const confirmed = window.confirm("Da li ste sigurni da želite da obrišete korisnika?");

    if (confirmed) {

      axios
        .delete(`http://localhost:5137/api/User/${userId}`, {
          headers: {
            Authorization: `Bearer ${userToken?.token}`,
          },
        })
        .then((response) => {
          if (response.status === 204) {
            alert(`Korisnik sa ID ${userId} je obrisan`);
            setUsers(users.filter((user) => user.usersId !== userId));
          }
        })
        .catch((error) => {
          console.error("Error deleting user:", error);
        });
    }
  };

  return (
    <div className="user-list">
      <h2>Lista korisnika</h2>
      <table>
        <thead>
          <tr>
            <th>Username</th>
            <th>Ime</th>
            <th>Prezime</th>
            <th>Email</th>
            <th>Broj telefona</th>
            <th>Uloga</th>
            <th>Izmeni</th>
            <th>Obrisi</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.usersId}>
              <td>{user.username}</td>
              <td>{user.firstName}</td>
              <td>{user.lastName}</td>
              <td>{user.email}</td>
              <td>{user.phone}</td>
              <td>{user.userRole}</td>
              <td>
                <Link to={`/editUser/${user.usersId}`}>Izmeni</Link>
              </td>
              <td>
                <button className="button-delete" onClick={() => deleteUser(user.usersId)}>Obriši</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ListUsers;
