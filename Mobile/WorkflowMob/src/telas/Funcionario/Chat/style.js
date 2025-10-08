import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({
 loginContainer: {
    flex: 1,
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "#f8f8f8ff",
    padding: 20,
  },
  botaoTexto: {
    color: "#ffffffff",
    fontSize: 16,
    fontWeight: "bold",
  },
  container: {
    flex: 1,
    backgroundColor: "#f6f5f5ff",
    padding: 10,
    paddingTop: 40,
  },
  header: {
    fontSize: 20,
    fontWeight: "bold",
    fontFamily: fonts.text,
    textAlign: "center",
    marginBottom: 10,
    color: "#333",
  },
  msg: {
    maxWidth: "75%",
    marginVertical: 5,
    padding: 10,
    borderRadius: 15,
  },
  msgMinha: {
    backgroundColor: "#4a90e2",
    alignSelf: "flex-end",
    borderBottomRightRadius: 0,
  },
  msgOutro: {
    backgroundColor: "#e2e6edff",
    alignSelf: "flex-start",
    borderBottomLeftRadius: 0,
  },
  usuario: {
    fontWeight: "bold",
    fontFamily: fonts.text,
    marginBottom: 3,
    color: "#444",
  },
  texto: {
    color: "#000",
    fontFamily: fonts.text,
    fontSize: 15,
  },
  linhaHora: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "flex-end",
    marginTop: 3,
  },
  hora: {
    fontSize: 11,
    fontFamily: fonts.text,
    color: "#666",
    marginRight: 5,
  },
  status: {
    fontSize: 12,
    fontFamily: fonts.text,
    color: "#666", 
  },
  statusLido: {
    color: "#1e90ff", 
  }, 
  inputArea: {
    flexDirection: "row",
    alignItems: "center",
    backgroundColor: "#fff",
    borderRadius: 30,
    paddingHorizontal: 10,
    paddingVertical: 5,
    margin: 10,
    elevation: 3,
    marginBottom:55,
  },
  inputMensagem: {
    flex: 1,
    paddingHorizontal: 15,
    fontSize: 16,
  },
  botaoEnviar: {
    backgroundColor: "#4a90e2",
    width: 45,
    height: 45,
    borderRadius: 25,
    justifyContent: "center",
    alignItems: "center",
  },
  input:{
    width: 200,
    borderColor: '#000',
    borderRadius: 10,
    borderWidth: 3
  },
  nav: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    paddingVertical: 15,
    marginBottom: 10,
    marginTop:15,
  },
  botaodevoltar: {
    width: 40,
    height: 40,
    justifyContent: 'center',
  },
  espacoHeader: {
    width: 40,
  },
  titulo: {
    width:150,
    height:50,
  },
  
});
