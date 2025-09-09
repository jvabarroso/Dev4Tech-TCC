import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, ActivityIndicator} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

import url from '../../../../services/url';

export default function HomeAdm({navigation, route}){
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const usuario = route.params?.usuario || {
    nome: 'Usuário não identificado',
    cargo: 'Cargo não definido',
  };
  console.log('Dados recebidos na Home:', route.params);

  const [usuarioState, setUsuarioState] = useState(usuario);
  const [imagens, setImagens] = useState([]);
  const [loading, setLoading] = useState(true);


//Mostra a foto do Usuario:
  useEffect(() => {
    if (!usuario.id || !usuario.role) return;

    async function carregarImagens() {
      try {
        const response = await fetch(`${url}/dev4tec/imagem_usuario.php`,{
            method:'POST',
            headers:{'Content-Type': 'application/json'},
            body: JSON.stringify({id: usuario.id, role: usuario.role})
          }
        );
        const data = await response.json();

        if (data.success) {
          setUsuarioState(prev => ({ ...prev, imagem: data.imagem }));
        }
      } catch (error) {
        console.error('Erro ao buscar imagens:', error);
      } finally {
        setLoading(false);
      }
    }

      carregarImagens();
  }, []);

  if (loading) {
    return (
      <View style={styles.loading}>
        <ActivityIndicator size="large" color="#4a90e2" />
        <Text style={styles.loadingText}>Carregando imagens...</Text>
      </View>
    );
  }
  console.log('Imagem do usuário:', usuarioState.imagem);

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <View style={styles.areaperfil}>
                <Image 
                  source={ 
                    usuarioState.imagem  
                    ? {uri: usuarioState.imagem}
                     : require('../../../../assets/img/fotoexemplo.png')}
                  style={styles.foto}
                />
                    <View style={styles.verde}></View>

                    <View style={styles.textoperfil}>
                        <Text style={styles.nome}>{usuario.nome}</Text>
                        <Text style={styles.profissao}>{usuario.cargo}</Text>
                    </View>
                </View>

                <View style={styles.areatitulo}>
                    <Text style={styles.titulo}>Home</Text>
                    <Text style={styles.subtitulo}>Explore as Ferramentas</Text>
                </View>

                <View style={styles.areacard}>
                    <TouchableOpacity
                        onPress={()=> navigation.navigate('CadastroFuncionario')} 
                    >
                        <Card style={styles.cardtarequi}>
                            <Card.Cover 
                                source={require('../../../../assets/img/cadastrarfuncionario.png')}
                                style={styles.imagemcard} />
                            <Card.Content style={styles.cardinferior}>
                                <Title style={styles.titulocard}>Cadastrar Funcionário</Title>
                                <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                                <View style={styles.linhainfer}>
                                    <Text style={styles.data}>16/07/20</Text>
                                    <Text style={styles.Entre}>Entre aqui</Text>
                                </View>
                            </Card.Content>
                        </Card>
                    </TouchableOpacity>
                    
                    <TouchableOpacity
                         onPress={()=> navigation.navigate('CadastroEquipes')} 
                    >
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../../assets/img/cadastrarequipes.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content style={styles.cardinferior}>
                            <Title style={styles.titulocard}>Cadastrar Equipes</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
                    </TouchableOpacity>

                    <TouchableOpacity
                         onPress={()=> navigation.navigate('CadastroTarefas')} 
                    >
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../../assets/img/tarefas.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content style={styles.cardinferior}>
                            <Title style={styles.titulocard}>Cadastrar Tarefas</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
                    </TouchableOpacity>

                    <TouchableOpacity
                        onPress={()=> navigation.navigate('RankingAdm')} 
                    >
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../../assets/img/ranking.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content style={styles.cardinferior}>
                            <Title style={styles.titulocard}>Ranking</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
                    </TouchableOpacity>
                    
                </View>
            </View>
        </ScrollView>
    )
}
