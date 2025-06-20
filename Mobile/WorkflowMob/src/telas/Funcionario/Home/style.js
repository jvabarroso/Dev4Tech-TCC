import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const getStyles = (theme) => StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: theme.background
  },
  scroll: {
    flex: 1,
    width: '100%',
  },
  areaperfil:{
    flexDirection: 'row',
    marginTop: 15,
    width: '100%',
    paddingHorizontal: 20,
  },
  foto:{
    width: 120,
    height: 120,
    marginLeft: 18,
    borderRadius: 60
  },
  verde:{
    width:20,
    height:20,
    borderRadius:"100%",
    borderWidth: 3.5,
    borderColor: '#F5F9F9',
    backgroundColor:"#2EBA4E",
    right:28,
    top:95,
  },
  textoperfil: {
    justifyContent: 'center',
    fontFamily: fonts.text,
  },
  nome: {
    color: theme.text,
    fontSize: 16,
    fontFamily: fonts.text,
    marginBottom: 2,
  },
  profissao: {
    color: theme.text2,
    fontSize: 16,
    fontFamily: fonts.text,
  },
  areatitulo:{
    justifyContent: 'center',
    marginRight: 50,
    marginTop: 20,
    marginBottom: 30,
  },
  titulo:{
    fontSize: 24,
    marginBottom: 4,
    color: theme.text,
    fontFamily: fonts.text,
  },
  subtitulo:{
    fontSize: 20,
    color: theme.text,
    fontFamily: fonts.text,
  },
  areacard:{
    marginBottom: 25,
  },
  cardtarequi:{
    marginBottom: 25,
    width:275,
  },
  imagemcard:{
    height:110,
  },
  cardinferior:{
    backgroundColor: theme.background,
    borderRadius:8,
    borderBottomWidth: -0.1,
    borderBottomColor: '#000',
  },
  titulocard:{
    color:theme.text,
    fontWeight: 'bold',
    fontFamily: fonts.text,
    marginBottom: -5,
  },
  paragraph:{
    color:theme.text,
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
    color:theme.text2,
  },
  Entre:{
    color:theme.text,
    fontSize: 11,
    fontFamily: fonts.text,
    fontWeight: 'bold',
  },
})