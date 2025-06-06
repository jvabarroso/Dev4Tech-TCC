import { StyleSheet } from "react-native";
import fonts from "../../../styles/fonts";



export const styles = StyleSheet.create({
   container: {
      flex: 1,
      backgroundColor: "#ffffff",
    },
    scrollView: {
      flex: 1,
    },
    containerConteudo: {
      paddingHorizontal: 20,
      paddingTop: 15,
      paddingBottom: 40,
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
    titulo: {
      fontSize: 18,
      fontFamily: fonts.text,
      fontWeight: 'bold',
      textAlign: 'center',
      flex: 1,
    },
    espacoHeader: {
      width: 40,
    },
    titulossub: {
      fontSize: 30,
      fontFamily: fonts.text,
      color: '#000',
      fontWeight: 'bold',
    },
    navinput: {
      width: '100%',
      padding: 10,
      fontSize: 17,
      fontFamily: fonts.text,
      backgroundColor: '#1C58F2',
      borderRadius: 10,
      borderBottomWidth: 0.1,
      borderBottomColor: '#000',
      marginBottom: 15,
      color: '#fff',
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
    colocacao:{
      fontSize: 28, 
      fontWeight: 'bold', 
      fontFamily: fonts.text,
      marginRight: 10,
    },
    containerestatisticas:{
      marginBottom: 20,
    },
    tituloestastisca: {
      fontSize: 30,
      fontFamily: fonts.text,
      color: '#000',
      fontWeight: 'bold',
      marginRight:150,
    },
    linha:{
      flexDirection: 'row',
      justifyContent: 'space-between',
      marginTop:10,
    },
    titulodetalhes:{
      fontSize: 20,
      fontFamily: fonts.text,
      color: '#000',
      fontWeight: 'bold',
      paddingHorizontal:5
    },
    containerbarras:{
      flexDirection: 'row',
      height: 200, 
      padding: 20,
    },
    colunagrafico:{
      gap:40,
      marginTop:7,
      marginRight: 10,
    },
    textobarras:{
      fontSize: 15,
      borderRadius:100,
    },
    barras:{
      flex:1,
    }

});
