import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#ffffff",
  },
  scrollContent: {
    padding: 16,
  },
  titulo: {
    fontSize: 30,
    color: '#000',
    fontWeight: 'bold',
    marginTop: '5%',
    marginBottom: "5%",
    fontFamily: fonts.text,
  },
  navinput: {
    width: '100%',
    padding: 10,
    fontSize: 17,
    backgroundColor: '#1A5CFF',
    borderRadius: 10,
    borderBottomWidth: 0.1,
    borderBottomColor: '#000',
    marginBottom: 15,
    color: '#fff',
  },
  flat: {
    flex: 1,
  },
  containertarefas: {
    backgroundColor: '#F5F7FC',
    borderRadius: 10,
    padding: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
  },
  imag: {
    width: 45,
    height: 45,
    marginLeft: 10,
  },
  textos: {
    marginLeft: 15,
    flex: 1,
  },
  textolistatitulo: {
    color: '#000',
    fontSize: 18,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  textolistacargo: {
    color: '#000',
    fontSize: 15,
    fontFamily: fonts.text,
  },
  areacard:{
    marginBottom: 25,
    alignSelf:"center",
  },
  cardtarequi:{
    marginBottom: 25,
    width:275,
  },
  imagemcard:{
    height:110,
  },
  cardinferior:{
    backgroundColor: "#ffffffff",
    borderRadius:8,
    borderBottomWidth: -0.1,
    borderBottomColor: '#000',
  },
  titulocard:{
    color:"#000",
    fontWeight: 'bold',
    fontFamily: fonts.text,
    marginBottom: -5,
  },
  paragraph:{
    color:"#000",
    fontFamily: fonts.text,
    fontSize: 11,
  },
  linhainfer:{
    flexDirection: 'row',
    marginTop: 11,
    justifyContent: 'space-between',
    width: '100%',
    paddingHorizontal: -10,
    marginBottom: -10,
  },
  data:{
    fontSize: 11,
    fontFamily: fonts.text,
    color: '#aaaaaa',
  },
  Entre:{
    color:"#000",
    fontSize: 11,
    fontFamily: fonts.text,
    fontWeight: 'bold',
  },
});
