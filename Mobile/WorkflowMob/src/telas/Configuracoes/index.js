import React, { useState, useEffect} from 'react';
import { Text, TextInput, View, TouchableOpacity, Image, ScrollView, Alert, ActivityIndicator, Modal} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../styles/themecontext'
import {Ionicons} from '@expo/vector-icons';

import api from '../../../services/api';
import * as ImagePicker from "expo-image-picker";
import FlashMessage, { showMessage } from "react-native-flash-message";

export default function Configuracoes({navigation, route}){
    const { theme, toggleTheme } = useTheme();
    const styles = getStyles(theme);

    const routeUsuario = route.params?.usuario || {};

    const initialUsuario = {  
      nome: 'Usuário não identificado',
      cargo: 'Cargo não definido',
      dataNascimento: "Data não definido",
      email:"Email não definido",
      telefone:'Telefone não definido',
      endereco:'Endereço não definido',
      cpf:"CPF não definido",
    };
    
  const usuario = {
    ...initialUsuario,
    ...routeUsuario,
    role: routeUsuario.role
  };

    const [dados, setDados] = useState([]);
    const [usuarioState, setUsuarioState] = useState(usuario);
    const [dataNascimento, setDataNascimento] = useState(usuario.dataNascimento);
    const [telefone, setTelefone] = useState(usuario.telefone);
    const [endereco, setEndereco] = useState(usuario.endereco);
    const [imagens, setImagens] = useState([]);
    const [loading, setLoading] = useState(true);
    const [modalVisivel, setModalVisivel] = useState(false);
    const [mostrardados, setMostrardados] = useState(false);
    const [image, setImage] = useState(null);

   function limparCampos(){
    setDataNascimento('');
    setTelefone('');
    setEndereco('');
   }
    //Máscara input
    // Adicione estas funções utilitárias no topo do arquivo
    function formatarDataParaBanco(data) {
      if (!data) return '';
      
      // Se já está no formato do banco, retorna direto
      if (/^\d{4}-\d{2}-\d{2}$/.test(data)) return data;
      
      // Converte de DD/MM/AAAA para AAAA-MM-DD
      const partes = data.split('/');
      if (partes.length === 3) {
        return `${partes[2]}-${partes[1]}-${partes[0]}`;
      }
      return data;
    }

    function formatarTelefone(telefone) {
      if (!telefone) return '';
      // Remove todos os caracteres não numéricos
      return telefone.replace(/\D/g, '');
    }

    function formatarDataInput(text) {
      let data = text.replace(/\D/g, '');
      
      if (data.length > 2) data = `${data.slice(0,2)}/${data.slice(2)}`;
      if (data.length > 5) data = `${data.slice(0,5)}/${data.slice(5,9)}`;
      
      return data.slice(0,10);
    }

    function formatarTelefoneInput(text) {
      let tel = text.replace(/\D/g, '');
      mostrardados
      if (tel.length > 0) tel = `(${tel}`;
      if (tel.length > 3) tel = `${tel.slice(0,3)}) ${tel.slice(3)}`;
      if (tel.length > 10) tel = `${tel.slice(0,10)}-${tel.slice(10,15)}`;
      
      return tel.slice(0,15);
    }

//Update Imagem
  async function pickImageFromGallery() {
      let result = await ImagePicker.launchImageLibraryAsync({
        mediaTypes: ImagePicker.MediaTypeOptions.Images,
        allowsEditing: true,
        aspect: [4, 3],
        quality: 1,
      });

      if (!result.canceled) {
        console.log(result); // Verificar o retorno completo
        setImage(result.assets[0].uri); // Acesse o URI corretamente
      }
    }

    async function takePhoto() {
      let result = await ImagePicker.launchCameraAsync({
        allowsEditing: true,
        aspect: [4, 3],
        quality: 1,
      });

      if (!result.canceled) {
        console.log(result); // Verificar o retorno completo
        setImage(result.assets[0].uri); // Acesse o URI corretamente
      }
    }

  async function uploadImage() {
      if (!image) {
        showMessage({
          message: 'Nenhuma imagem selecionada.',
          description: 'Por favor, selecione ou tire uma foto primeiro.',
          floating: true,
          statusBarHeight: 70,
          type: "danger",
          duration: 2000,             
        });
        return;
      }

      let filename = image.split('/').pop();
      let match = /\.(\w+)$/.exec(filename);
      let type = match ? `image/${match[1]}` : `image`;

      let formData = new FormData();
      formData.append('photo', { uri: image, name: filename, type });
      formData.append("role", usuarioState.role);
      formData.append("id", usuarioState.id);

      try {
        const response = await fetch("http://10.239.0.125/dev4tec/upload_usuario.php", {
          method: 'POST',
          body: formData,
        });
           
        const text = await response.text();
        console.log("RESPOSTA DO PHP:", text);

        let resJson;
        try {
          resJson = JSON.parse(text);
        } catch (e) {
          console.error("Erro ao converter JSON:", e);
        }

        if (response.ok && resJson.success) {        
          showMessage({
            message: 'Sucesso.',
            description: 'Imagem enviada com sucesso!',
            floating: true,
            statusBarHeight: 70,
            type: "success",
            duration: 2000,             
          });
        } else {
            showMessage({
              message: 'Erro.',
              description: resJson.message || "Falha ao enviar imagem.",
              floating: true,
              statusBarHeight: 70,
              type: "warning",
              duration: 2000,             
            });
        }
      } catch (error) {
          console.error(error);
          showMessage({
            message: 'Erro.',
            description: "Ocorreu um erro ao tentar enviar a imagem.",
            floating: true,
            statusBarHeight: 70,
            type: "warning",
            duration: 2000,             
          });
      }
    }
    console.log('Dados recebidos nas Configuraçõpes:', route.params);
//Mostra a foto do Usuario:
  useEffect(() => {
    async function carregarImagens() {
      try {
        const response = await fetch(
          `http://10.239.0.125/dev4tec/imagem_usuario.php`,{
            method:'POST',
            headers:{'Content-Type': 'application/json'},
            body: JSON.stringify({ id: usuarioState.id, role: usuarioState.role })
          }
        );
        const data = await response.json();

        if (data.success) {
          setUsuarioState(prev => ({
            ...prev,
            imagem: data.imagem,
            dataNascimento,
            telefone,
            endereco,
          }));
        }
      } catch (error) {
        console.error('Erro ao buscar imagens:', error);
      } finally {
        setLoading(false);
      }
    }

      carregarImagens();
  }, [usuarioState.id]);

  if (loading) {
    return (
      <View style={styles.loading}>
        <ActivityIndicator size="large" color="#4a90e2" />
        <Text style={styles.loadingText}>Carregando imagens...</Text>
      </View>
    );
  }


//Post para o Banco:
    async function editar() {            
        if (!dataNascimento|| !telefone || !endereco) {
        Alert.alert("Erro", "Preencha todos os campos obrigatórios!");
         return;
     }
     try {
        const dataFormatada = formatarDataParaBanco(dataNascimento);
        const telefoneFormatado = formatarTelefone(telefone);

        const obj = {
          id: usuarioState.id,
          role: usuarioState.role, // Usando o estado atualizado
          DataNascimento: dataFormatada,
          Telefone: telefoneFormatado, 
          endereco: endereco,
        };

         console.log('Dados enviados para edição:', obj); // Log para debug

          const res = await api.post('dev4tec/editardados.php', obj, {
            headers: {
              'Content-Type': 'application/json',
            }
          });

                  
         console.log('Resposta da API:', res.data); // Log para 
         
        if (res.data.success) {
          setUsuarioState(prev => ({
            ...prev,
            dataNascimento: dataNascimento,
            telefone: telefone,
            endereco: endereco
              }));
            Alert.alert("Sucesso", "Dados atualizados com sucesso!");
        } else {
            Alert.alert("Erro", res.data.message || "Erro ao atualizar dados");
        }
    } catch (error) {
        console.error("Erro completo:", error);
        Alert.alert("Erro", "Não foi possível conectar ao servidor");
    }
}

    return(
      <View style={styles.container}>
        <ScrollView 
          contentContainerStyle={styles.scrollContent } keyboardShouldPersistTaps="handled">
          <View style={styles.nav}>
            <Text style={styles.logo}>WORKFLOW</Text>
          </View>
            <Text style={styles.titulo}>Configurações</Text>

            <View style={styles.linha}> 
              <TouchableOpacity
                style={styles.botaodevoltar}
                onPress={()=> navigation.goBack()}
              >
                <Ionicons name="chevron-back-outline" size={20} color={theme.text} style={styles.botaodevoltar}/>
              </TouchableOpacity>

              <Text style={styles.pontuacao}>
                Pontuação:<Text style={styles.pontuacaotext}>100</Text>
              </Text>
            </View>

            <View style={styles.containerfuncionario}>
              <TouchableOpacity onPress={() => setModalVisivel(true)}>
                <Image 
                  source={usuarioState.imagem ? { uri: usuarioState.imagem } :require('../../../assets/img/image.png')} 
                  style={styles.imagemfuncionario} />
              </TouchableOpacity>

              <View style={styles.textos}>
                <Text style={styles.textofuncionario}>{usuarioState.nome}</Text>
                <Text style={styles.textofuncionariocargo}>{usuarioState.cargo}</Text>
              </View>
            </View>

            <View style={styles.linha}>
              <TouchableOpacity 
                style={styles.inputfuncionario}
                onPress={() => setMostrardados(!mostrardados)}
              >
                <Text style={styles.textobotao3}>{mostrardados ? 'Fechar Dados pessoais ' : 'Ver Dados pessoais'}</Text>
              </TouchableOpacity>              
              <TouchableOpacity
                style={styles.botaomodo} 
                onPress={toggleTheme}
              >
                <Text style={styles.textobotao3}>
                  { theme.mode === 'dark' ? 'Modo Claro' : 'Modo Escuro' }
                </Text>
              </TouchableOpacity>
            </View>

            {mostrardados && (
              <View style={styles.areaInput}>

                <Text style={styles.titulo2}>Dados:</Text>

                <Text style={styles.texto}>Nome</Text>
                <View style={styles.inputnaoeditavel}>
                  <Text style={{ color: theme.text3 }}>{usuarioState.nome}</Text>
                </View>

                <Text style={styles.texto}>Data de nascimento</Text>
                <TextInput
                  style={styles.input}
                  value={dataNascimento}
                  placeholder={usuarioState.dataNascimento}
                  placeholderTextColor={theme.text2}
                  onChangeText={(text) => setDataNascimento(formatarDataInput(text))}
                  keyboardType="numeric"
                  maxLength={10}
                />
                
                <Text style={styles.texto}>CPF</Text>
                <View style={styles.inputnaoeditavel}>  
                  <Text style={{ color: theme.text3 }}>{usuarioState.cpf}</Text>
                </View>

                <Text style={styles.texto}>Email</Text>
                 <View style={styles.inputnaoeditavel}>
                  <Text style={{ color: theme.text3 }}>{usuarioState.email}</Text>
                </View>

                <Text style={styles.texto}>Telefone</Text>
                <TextInput
                  style={styles.input}
                  value={telefone}
                  placeholder={usuarioState.telefone}
                  placeholderTextColor={theme.text2}
                  onChangeText={(text) => setTelefone(formatarTelefoneInput(text))}
                  keyboardType="phone-pad"
                  maxLength={15}
                />

                <Text style={styles.texto}>Endereço</Text>
                <TextInput
                  style={styles.input}
                  placeholder={usuarioState.endereco}
                  placeholderTextColor={theme.text2}
                  onChangeText={(text) => setEndereco(text)}
                />

                <TouchableOpacity 
                  style={styles.botaoeditar}
                  onPress={() => {
                    editar()
                  }}
                >    
                  <Text style={styles.textoeditar}>Editar dados</Text>                     
                </TouchableOpacity>
              </View>
            )}


            <TouchableOpacity 
              style={styles.botaosair}
              onPress={()=> navigation.navigate('Login')} 
            >    
              <Text style={styles.textoeditar}>Sair da conta</Text>                     
            </TouchableOpacity>
        </ScrollView>
        <Modal
          animationType="slide"
          transparent={true}
          visible={modalVisivel}
          onRequestClose={() => setModalVisivel(false)}
        >   
          <View style={styles.modalContainer}>
            <View style={styles.modalContent}>
              <View style={styles.nav2}>
                <TouchableOpacity 
                  style={styles.botaodevoltar}
                  onPress={() => setModalVisivel(false)}
                >
                <Ionicons name="close-outline" size={36} color={theme.text} />
                </TouchableOpacity>
              </View>
              <View style={styles.areafotototal}>
                <View style={styles.areatitulofoto}>
                  <Text style={styles.textfoto}>Selecione uma foto</Text>
                </View>
                <View style={styles.areafoto}>

                  <TouchableOpacity 
                    style={styles.buttonEnviar} 
                    onPress={uploadImage}
                  >
                    <Ionicons name="cloud-upload" size={20} color="white"/>
                    <Text style={styles.buttonText}>Enviar Imagem</Text>
                  </TouchableOpacity>

                  <View style={styles.areafoto2}>
                    <TouchableOpacity 
                      style={styles.button} 
                      onPress={pickImageFromGallery}
                    >
                      <Text style={styles.buttonText2}>Escolher da Galeria</Text>
                    </TouchableOpacity>
              
                    <TouchableOpacity 
                      style={styles.button} 
                      onPress={takePhoto}
                    >   
                      <Text style={styles.buttonText2}>Tirar Foto</Text>
                    </TouchableOpacity>   
                  </View>
                </View>
              </View>
            </View>
           <FlashMessage position="top" />
          </View>
        </Modal>
      </View>
    )
}
